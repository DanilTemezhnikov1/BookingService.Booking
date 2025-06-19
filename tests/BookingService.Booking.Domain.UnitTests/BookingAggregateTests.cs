using BookingService.Booking.Domain.Bookings;
using BookingService.Booking.Domain.Contracts.Bookings;
using BookingService.Booking.Domain.Exceptions;

namespace BookingService.Booking.Domain.UnitTests;

public class BookingAggregateTests
{
    [Fact]
    public void Ctor_valid_all_arguments_creates_with_correct_ids_and_dates()
    {
        long idUser = 5;
        long idBooking = 5;
        var startBooking = new DateOnly(2026, 3, 5);
        var endBooking = new DateOnly(2026, 4, 5);
        var creationBooking = new DateTimeOffset(new DateTime(2025, 2, 1));


        var bookingAggregate =
            BookingAggregate.Initialize(idUser, idBooking, startBooking, endBooking, creationBooking);


        Assert.Equal(idUser, bookingAggregate.IdUser);
        Assert.Equal(idBooking, bookingAggregate.IdBooking);
        Assert.Equal(startBooking, bookingAggregate.StartBooking);
        Assert.Equal(endBooking, bookingAggregate.EndBooking);
        Assert.Equal(creationBooking, bookingAggregate.CreationBooking);
    }

    [Fact]
    public void Ctor_id_user_less_than_or_equal_to_zero_throws_DE()
    {
        long idUser = 0;
        long idBooking = 14;


        Assert.Throws<DomainException>(() =>
            BookingAggregate.Initialize(idUser, idBooking, new DateOnly(), new DateOnly(), new DateTimeOffset()));
    }

    [Fact]
    public void Ctor_id_booking_less_than_or_equal_to_zero_throws_AE()
    {
        long idUser = 7;
        long idBooking = -92;


        Assert.Throws<DomainException>(() =>
            BookingAggregate.Initialize(idUser, idBooking, new DateOnly(), new DateOnly(), new DateTimeOffset()));
    }

    [Fact]
    public void Ctor_start_booking_less_than_or_equal_to_creation_booking_throws_DE()
    {
        var startBooking = new DateOnly(2004, 6, 30);
        var creationBooking = new DateTimeOffset(new DateTime(2025, 2, 1));


        Assert.Throws<DomainException>(() =>
            BookingAggregate.Initialize(long.MaxValue, long.MaxValue, startBooking, new DateOnly(), creationBooking));
    }

    [Fact]
    public void Ctor_end_booking_less_than_or_equal_to_start_booking_throws_DE()
    {
        var startBooking = new DateOnly(2025, 12, 12);
        var endBooking = new DateOnly(2004, 6, 30);


        Assert.Throws<DomainException>(() =>
            BookingAggregate.Initialize(long.MaxValue, long.MaxValue, startBooking, endBooking, new DateTimeOffset()));
    }

    [Fact]
    public void Confirm_status_awaits_confirmation()
    {
        //после инициализации статус будет AwaitsConfirmation
        var bookingAggregate = BookingAggregate.Initialize(long.MaxValue, long.MaxValue, new DateOnly(2002, 2, 2),
            new DateOnly(2003, 3, 3), new DateTimeOffset(new DateTime(2001, 1, 1)));


        bookingAggregate.Confirm();


        Assert.Equal(BookingStatus.Confirmed, bookingAggregate.Status);
    }

    [Fact]
    public void Confirm_status_confirmed_throws_DE()
    {
        var bookingAggregate = BookingAggregate.Initialize(long.MaxValue, long.MaxValue, new DateOnly(2002, 2, 2),
            new DateOnly(2003, 3, 3), new DateTimeOffset(new DateTime(2001, 1, 1)));
        bookingAggregate.Confirm();


        Assert.Throws<DomainException>(() => bookingAggregate.Confirm());
    }

    [Fact]
    public void Cancel_status_confirmed()
    {
        var bookingAggregate = BookingAggregate.Initialize(long.MaxValue, long.MaxValue, new DateOnly(2002, 2, 2),
            new DateOnly(2003, 3, 3), new DateTimeOffset(new DateTime(2001, 1, 1)));
        bookingAggregate.Confirm();


        bookingAggregate.Cancel();


        Assert.Equal(BookingStatus.Cancelled, bookingAggregate.Status);
    }

    [Fact]
    public void Cancel_status_cancelled_throws_DE()
    {
        var bookingAggregate = BookingAggregate.Initialize(long.MaxValue, long.MaxValue, new DateOnly(2002, 2, 2),
            new DateOnly(2003, 3, 3), new DateTimeOffset(new DateTime(2001, 1, 1)));
        bookingAggregate.Cancel();


        Assert.Throws<DomainException>(() => bookingAggregate.Cancel());
    }
}