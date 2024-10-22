using BookRentalApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookRentalApi.Data
{
    public class BookRentalDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public BookRentalDbContext(DbContextOptions<BookRentalDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .Property(b => b.RowVersion)
                .IsRowVersion();  // For concurrency handling
        }
    }
}
