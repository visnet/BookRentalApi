using BookRentalApi.Models;

namespace BookRentalApi.Services
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> SearchBooks(string name, string genre);
        Task RentBook(int bookId, int userId);
        Task ReturnBook(int bookId, int userId);
        Task<IEnumerable<Rental>> GetRentalHistory(int userId);
        Task MarkOverdueRentals();
        Task<BookStats> GetBookStats();
    }
}
