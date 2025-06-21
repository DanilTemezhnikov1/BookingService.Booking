using BookingService.Booking.AppServices.Bookings.Jobs;
using BookingService.Booking.AppServices;

namespace BookingService.Booking.Host;

public static class ServiceCollectionExtensions
{
    public static void AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<IBookingsBackgroundServiceHandler, BookingsBackgroundServiceHandler>();
        services.AddHostedService<BookingsBackgroundService>();
    }
}
