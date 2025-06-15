using BookingService.Booking.AppServices.Bookings.Jobs;

namespace BookingService.Booking.Persistence
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<IBookingsBackgroundServiceHandler, BookingsBackgroundServiceHandler>();
            services.AddHostedService<BookingsBackgroundService>();
        }
    }
}
