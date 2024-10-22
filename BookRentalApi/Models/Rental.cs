namespace BookRentalApi.Models
{
    public class Rental
    {
        public int RentalId { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime RentDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsReturned { get; set; }
        public bool IsOverdue { get; set; }

        public User User { get; set; }
        public Book Book { get; set; }
    }

}
