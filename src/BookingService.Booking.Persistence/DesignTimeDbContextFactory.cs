using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BookingService.Booking.Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BookingsContext>
{
    public BookingsContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BookingsContext>();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", false, true)
            .Build();

        var connectionString = configuration.GetConnectionString(nameof(BookingsContext));
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException($"ConnectionString for `{nameof(BookingsContext)}` not found");

        optionsBuilder.UseNpgsql(connectionString);

        return new BookingsContext(optionsBuilder.Options);
    }
}