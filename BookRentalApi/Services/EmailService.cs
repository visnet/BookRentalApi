using BookRentalApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using System;
using System.Threading.Tasks;

namespace BookRentalApi.Services
{



    public class EmailService
    {
        private readonly string _smtpServer;
        private readonly int _port;
        private readonly string _fromAddress;
        private readonly string _password;
        private readonly BookRentalDbContext _context;

        public EmailService(IConfiguration configuration, BookRentalDbContext context)
        {
            _smtpServer = configuration["EmailSettings:SMTPServer"];
            _port = int.Parse(configuration["EmailSettings:Port"]);
            _fromAddress = configuration["EmailSettings:Username"];
            _password = configuration["EmailSettings:Password"];
            _context = context;
        }

        public async Task SendEmailAsync(string toAddress, string subject, string body)
        {
            using var client = new SmtpClient(_smtpServer, _port)
            {
                Credentials = new NetworkCredential(_fromAddress, _password),
                EnableSsl = true
            };

            var mailMessage = new MailMessage(_fromAddress, toAddress, subject, body);
            await client.SendMailAsync(mailMessage);
        }



        // Method for sending overdue notifications
        public async Task SendOverdueNotification(int userId, int bookId)
        {
            // Fetch user information from the database
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            // Fetch book information from the database
            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
            {
                throw new Exception("Book not found.");
            }

            // Prepare the email content
            string userEmail = user.Email;  // Assuming the User model has an Email property
            string subject = "Overdue Book Notification";
            string body = $"Dear {user.Name.ToUpper()},  The book '{book.Title.ToUpper()}' (ID: {bookId}) is overdue. Please return it as soon as possible. Thank you!";

            // Send the email notification
            await SendEmailAsync(userEmail, subject, body);
        }
    }
}

