using BookingService.Booking.AppServices.Dates;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookingService.Booking.AppServices.Bookings
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAppServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IBookingsService, BookingService>();
            services.AddSingleton<ICurrentDateTimeProvider, DefaultCurrentDateTimeProvider>();
        }
    }
}
