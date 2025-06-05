using BookingService.Booking.AppServices.Queries;
namespace BookingService.Booking.AppServices.Bookings
{
    public interface IBookingsQueries
    {
        Task<BookingData[]> GetByFilter(GetBookingsByFilterQuery getBookingsByFilter);
        Task<string> GetStatusById(long? id);
    }
}
