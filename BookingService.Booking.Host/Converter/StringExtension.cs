using BookingService.Booking.AppServices.Bookings;

namespace BookingService.Booking.Host.Converter
{
    public static class StringExtension
    {
        public static BookingStatus ToEnum(this string value) =>
               value switch
               {
                   "AwaitsConfirmation" => BookingStatus.AwaitsConfirmation,
                   "Confirmed" => BookingStatus.Confirmed,
                   "Cancelled" => BookingStatus.Cancelled,
                   _ => throw new ArgumentException()
               };

    }
}
