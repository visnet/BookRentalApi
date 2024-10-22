using BookRentalApi.Data;
using BookRentalApi.Exceptions;
using BookRentalApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookRentalApi.Services
{
    public class BookService : IBookService
    {
        private readonly BookRentalDbContext _context;
        private readonly EmailService _emailService;

        public BookService(BookRentalDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<IEnumerable<Book>> SearchBooks(string name, string genre)
        {
            return await _context.Books
                .Where(b => (string.IsNullOrEmpty(name) || b.Title.Contains(name)) &&
                            (string.IsNullOrEmpty(genre) || b.Genre == genre))
                .ToListAsync();
        }

        public async Task RentBook(int bookId, int userId)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
            {
                throw new BookNotFoundException(bookId);
            }

            _context.Entry(book).Property(b => b.RowVersion).OriginalValue = book.RowVersion;

            if (book.AvailableCopies <= 0)
                throw new BookNotFoundException(bookId);

            book.AvailableCopies--;

            var rental = new Rental
            {
                BookId = bookId,
                UserId = userId,
                RentDate = DateTime.Now
            };

            _context.Rentals.Add(rental);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new Exception("The book was updated by another user. Please try again.");
            }
        }


        public async Task ReturnBook(int bookId, int userId)
        {
            var rental = await _context.Rentals
                .Where(r => r.BookId == bookId && r.UserId == userId && !r.IsReturned)
                .FirstOrDefaultAsync();

            if (rental == null)
                throw new RentalException($"No active rental found for book ID {bookId} and user ID {userId}.");

            rental.IsReturned = true;
            rental.ReturnDate = DateTime.Now;

            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
            {
                throw new BookNotFoundException(bookId);
            }
            book.AvailableCopies++;

            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Rental>> GetRentalHistory(int userId)
        {
            return await _context.Rentals
                .Include(r => r.Book)
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }
        public async Task MarkOverdueRentals()
        {
            var overdueRentals = await _context.Rentals
                .Where(r => !r.IsReturned && r.RentDate.AddMinutes(1) < DateTime.Now)
                .ToListAsync();

            foreach (var rental in overdueRentals)
            {
                rental.IsOverdue = true;
                // Send overdue notification to the user
                await _emailService.SendOverdueNotification(rental.UserId, rental.BookId);
            }

            await _context.SaveChangesAsync();
        }
        public async Task<BookStats> GetBookStats()
        {
            var mostPopularBook = await _context.Books
                .OrderByDescending(b => _context.Rentals.Count(r => r.BookId == b.BookId))
                .FirstOrDefaultAsync();

            var leastPopularBook = await _context.Books
                .OrderBy(b => _context.Rentals.Count(r => r.BookId == b.BookId))
                .FirstOrDefaultAsync();

            var mostOverdueBook = await _context.Books
                .OrderByDescending(b => _context.Rentals.Count(r => r.BookId == b.BookId && r.IsOverdue))
                .FirstOrDefaultAsync();

            return new BookStats
            {
                MostPopularBook = mostPopularBook,
                LeastPopularBook = leastPopularBook,
                MostOverdueBook = mostOverdueBook
            };
        }



    }
}
