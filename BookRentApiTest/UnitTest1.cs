using BookRentalApi.Data;
using BookRentalApi.Models;
using BookRentalApi.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Moq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using BookRentalApi.Controllers;


namespace BookRentApiTest
{
  
    public class BookControllerTests
    {
        private readonly Mock<IBookService> _bookServiceMock;
        private readonly BookController _bookController;

        public BookControllerTests()
        {
            // Arrange: Set up the mock and controller
            _bookServiceMock = new Mock<IBookService>();
            _bookController = new BookController(_bookServiceMock.Object);
        }

        // Test for the SearchBooks action
        [Fact]
        public async Task SearchBooks_ShouldReturnOk_WithBooks()
        {
            // Arrange
            var books = new List<Book>
        {
            new Book { BookId = 1, Title = "1984", Genre = "Dystopian", Author = "George Orwell" },
            new Book { BookId = 2, Title = "The Hobbit", Genre = "Fantasy", Author = "J.R.R. Tolkien" }
        };

            _bookServiceMock.Setup(s => s.SearchBooks(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(books);

            // Act
            var result = await _bookController.SearchBooks("1984", "Dystopian");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBooks = Assert.IsType<List<Book>>(okResult.Value);
            Assert.Equal(2, returnedBooks.Count);
        }

        // Test for the RentBook action
        [Fact]
        public async Task RentBook_ShouldReturnOk_WhenBookIsRented()
        {
            // Arrange
            var rentRequest = new RentRequest { BookId = 1, UserId = 123 };

            _bookServiceMock.Setup(s => s.RentBook(rentRequest.BookId, rentRequest.UserId)).Returns(Task.CompletedTask);

            // Act
            var result = await _bookController.RentBook(rentRequest);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }

        // Test for the ReturnBook action
        [Fact]
        public async Task ReturnBook_ShouldReturnOk_WhenBookIsReturned()
        {
            // Arrange
            var returnRequest = new ReturnRequest { BookId = 1, UserId = 123 };

            _bookServiceMock.Setup(s => s.ReturnBook(returnRequest.BookId, returnRequest.UserId)).Returns(Task.CompletedTask);

            // Act
            var result = await _bookController.ReturnBook(returnRequest);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }

        // Test for the ViewRentalHistory action
        [Fact]
        public async Task ViewRentalHistory_ShouldReturnOk_WithRentalHistory()
        {
            // Arrange
            var rentalHistory = new List<Rental>
        {
            new Rental { BookId = 1, UserId = 123, IsReturned = true },
            new Rental { BookId = 2, UserId = 123, IsReturned = false }
        };

            _bookServiceMock.Setup(s => s.GetRentalHistory(It.IsAny<int>())).ReturnsAsync(rentalHistory);

            // Act
            var result = await _bookController.ViewRentalHistory(123);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var history = Assert.IsType<List<Rental>>(okResult.Value);
            Assert.Equal(2, history.Count);
        }

        // Test for the GetBookStats action
        [Fact]
        public async Task GetBookStats_ShouldReturnOk_WithStats()
        {
            // Arrange
            var stats = new BookStats
            {
                MostPopularBook = new Book { BookId = 1, Title = "1984" },
                LeastPopularBook = new Book { BookId = 2, Title = "The Hobbit" },
                MostOverdueBook = new Book { BookId = 3, Title = "The Catcher in the Rye" }
            };

            _bookServiceMock.Setup(s => s.GetBookStats()).ReturnsAsync(stats);

            // Act
            var result = await _bookController.GetBookStats();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedStats = Assert.IsType<BookStats>(okResult.Value);
            Assert.NotNull(returnedStats.MostPopularBook);
            Assert.NotNull(returnedStats.LeastPopularBook);
            Assert.NotNull(returnedStats.MostOverdueBook);
        }

        // Test for the MarkOverdueRentals action
        [Fact]
        public async Task MarkOverdueRentals_ShouldReturnOk_WhenOverdueRentalsMarked()
        {
            // Arrange
            _bookServiceMock.Setup(s => s.MarkOverdueRentals()).Returns(Task.CompletedTask);

            // Act
            var result = await _bookController.MarkOverdueRentals();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Overdue rentals marked and notifications sent.", ((dynamic)okResult.Value).message);
        }

        // Test for the MarkOverdueRentals action when exception occurs
        [Fact]
        public async Task MarkOverdueRentals_ShouldReturn500_WhenErrorOccurs()
        {
            // Arrange
            _bookServiceMock.Setup(s => s.MarkOverdueRentals()).Throws(new Exception("Test Exception"));

            // Act
            var result = await _bookController.MarkOverdueRentals();

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("An error occurred while marking overdue rentals.", ((dynamic)objectResult.Value).message);
            Assert.Equal("Test Exception", ((dynamic)objectResult.Value).details);
        }
    }



}