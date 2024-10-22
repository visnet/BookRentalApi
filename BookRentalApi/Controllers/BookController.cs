using BookRentalApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookRentalApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchBooks([FromQuery] string name, [FromQuery] string genre)
        {
            var books = await _bookService.SearchBooks(name, genre);
            return Ok(books);
        }

        [HttpPost("rent")]
        public async Task<IActionResult> RentBook([FromBody] RentRequest rentRequest)
        {
            await _bookService.RentBook(rentRequest.BookId, rentRequest.UserId);
            return Ok();
        }

        [HttpPost("return")]
        public async Task<IActionResult> ReturnBook([FromBody] ReturnRequest returnRequest)
        {
            await _bookService.ReturnBook(returnRequest.BookId, returnRequest.UserId);
            //await _bookService.MarkOverdueRentals();
            return Ok();
        }
        [HttpGet("history/{userId}")]
        public async Task<IActionResult> ViewRentalHistory(int userId)
        {
            var history = await _bookService.GetRentalHistory(userId);
            return Ok(history);
        }
        [HttpGet("stats")]
        public async Task<IActionResult> GetBookStats()
        {
            var stats = await _bookService.GetBookStats();
            return Ok(stats);
        }
        // Endpoint to mark overdue rentals
        [HttpPost("mark-overdue")]
        public async Task<IActionResult> MarkOverdueRentals()
        {
            try
            {
                await _bookService.MarkOverdueRentals();
                return Ok(new { message = "Overdue rentals marked and notifications sent." });
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here)
                return StatusCode(500, new { message = "An error occurred while marking overdue rentals.", details = ex.Message });
            }
        }


    }

    public class RentRequest
    {
        public int BookId { get; set; }
        public int UserId { get; set; }
    }

    public class ReturnRequest
    {
        public int BookId { get; set; }
        public int UserId { get; set; }
    }

}
