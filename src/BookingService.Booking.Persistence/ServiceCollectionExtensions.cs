using BookingService.Booking.Domain;
using BookingService.Booking.Domain.Bookings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BookingService.Booking.AppServices;

namespace BookingService.Booking.Persistence;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, string connectionString)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentNullException(nameof(connectionString));

        services.AddScoped<IBookingsRepository, BookingsRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IBookingsQueries, BookingQueries>();

        services.AddDbContext<BookingsContext>((ctx, context) =>
            {
                context.UseNpgsql(connectionString)
                    .UseLoggerFactory(ctx.GetRequiredService<ILoggerFactory>());
            }
        );

        return services;
    }
}