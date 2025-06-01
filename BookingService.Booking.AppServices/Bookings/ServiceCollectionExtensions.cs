using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Booking.AppServices.Bookings
{
    internal static class ServiceCollectionExtensions
    {
        static void AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<IBookingsService, BookingService>();
            services.AddScoped<IBookingsQueries, BookingQueries>();
        }
    }
}
