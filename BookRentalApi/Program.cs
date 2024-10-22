using BookRentalApi.Data;
using BookRentalApi.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;

namespace BookRentalApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            Log.Logger = new LoggerConfiguration()
             .WriteTo.Console()
            .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

            builder.Host.UseSerilog();


            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            builder.Services.AddDbContext<BookRentalDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("myconn")));

           
            builder.Services.AddScoped<IBookService, BookService>();
            //builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
            builder.Services.AddTransient<EmailService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
