using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Booking.Api.Contracts.Bookings
{
    internal static class WebRoutes
    {
        public const string BasePath = "api / bookings";
        public const string Create = BasePath + "/create";
        public const string GetById = BasePath + "/{id}";
        public const string Cancel = BasePath + "/{id}/cancel";
        public const string GetByFilter = BasePath + "/by-filter";
        public const string GetStatusById = BasePath + "{id}/status";
    }
}
