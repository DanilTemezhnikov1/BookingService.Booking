using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Booking.AppServices.Dates
{
    public class DefaultCurrentDateTimeProvider : ICurrentDateTimeProvider
    {
        public DateTimeOffset Now => DateTimeOffset.Now.ToLocalTime();
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}
