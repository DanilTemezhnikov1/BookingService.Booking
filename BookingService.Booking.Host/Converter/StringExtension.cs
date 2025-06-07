using BookingService.Booking.Domain.Contracts;

namespace BookingService.Booking.Host.Converter
{
    public static class StringExtension
    {
        public static BookingStatus ToBookingStatus(this string value) =>
               value switch
               {
                   "AwaitsConfirmation" => BookingStatus.AwaitsConfirmation,
                   "Confirmed" => BookingStatus.Confirmed,
                   "Cancelled" => BookingStatus.Cancelled,
                   _ => throw new ArgumentException()
               };

    }
}
