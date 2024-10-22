# Book Rental API

This is a RESTful API built with .NET Core for a book rental service. The API allows users to search for books, rent books, return them, and view their rental history. It also manages overdue books, sends email notifications, and provides book statistics like most popular, least popular, and most overdue books. The API ensures robust error handling, performance monitoring, and supports concurrent user access.

## Features

- **Search for Books** by Name and/or Genre
- **Rent a Book** and return it
- **View Rental History** for users
- **Mark Overdue Rentals** automatically if books are not returned within two weeks
- **Email Notifications** sent to users when their rentals become overdue
- **Book Statistics**: Most overdue book, most popular, and least popular book
- **Concurrency Handling**: Ensures concurrent access for at least 5 users
- **Error Handling**: Comprehensive validation and error management
- **Performance Monitoring**: Logs system activity and tracks API performance

## Bonus Features (Optional)

- **Waiting List** for renting books once they become available
- **Book Reservation** notifications for desired books
- **Rental Period Extensions** with overdue tracking

## Tech Stack

- **Backend**: ASP.NET Core 6
- **Database**: SQL Server
- **Email Service**: SMTP for sending email notifications
- **ORM**: Entity Framework Core
- **Testing**: xUnit and Moq for unit testing
- **Logging & Monitoring**: Basic logging for events and performance

## Requirements

- .NET 6 SDK or later
- SQL Server
- SMTP Server (for email notifications)

## Getting Started

### 1. Clone the Repository


git clone https://github.com/your-username/BookRentalAPI.git
cd BookRentalAPI

Update the connection string in the appsettings.json file to point to your SQL Server instance:
Run Migrations
Configure the SMTP settings in appsettings.json:

"SmtpSettings": {
  "Host": "smtp.your-email.com",
  "Port": 587,
  "Username": "your-email@domain.com",
  "Password": "your-password",
  "FromEmail": "your-email@domain.com"
}

