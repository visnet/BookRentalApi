namespace BookRentalApi.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public int UserId { get; set; }
        public int RentalId { get; set; }
        public DateTime SentDate { get; set; }
        public string NotificationType { get; set; }
    }

}
