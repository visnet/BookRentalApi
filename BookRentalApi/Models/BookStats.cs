namespace BookRentalApi.Models
{
    public class BookStats
    {
        public Book MostPopularBook { get; set; }
        public Book LeastPopularBook { get; set; }
        public Book MostOverdueBook { get; set; }
    }
}
