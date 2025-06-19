using BookingService.Booking.AppServices.Dates;
using BookingService.Booking.AppServices.Queries;
using BookingService.Booking.Domain;
using BookingService.Booking.Domain.Bookings;

namespace BookingService.Booking.AppServices.Bookings;

internal class BookingService : IBookingsService
{
    private readonly IBookingsRepository _bookingsRepository;
    private readonly ICurrentDateTimeProvider _currentDateTimeProvider;
    private readonly IUnitOfWork _unitOfWork;

    public BookingService(IUnitOfWork unitOfWork, ICurrentDateTimeProvider timeProvider)
    {
        _unitOfWork = unitOfWork;
        _bookingsRepository = _unitOfWork.BookingsRepository;
        _currentDateTimeProvider = timeProvider;
    }

    public async Task<long> Create(CreateBookingQuery createBooking, CancellationToken cancellationToken)
    {
        var aggregate = BookingAggregate.Initialize(
            createBooking.IdUser,
            createBooking.IdBooking,
            createBooking.StartBooking,
            createBooking.EndBooking,
            _currentDateTimeProvider.UtcNow);
        await _bookingsRepository.Create(aggregate, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
        return aggregate.Id;
    }

    public async Task<BookingData> GetById(long id, CancellationToken cancellationToken)
    {
        var aggregate = await _bookingsRepository.GetById(id, cancellationToken);
        return aggregate.ToBookingData();
    }

    public async Task Cancel(long id, CancellationToken cancellationToken)
    {
        var aggregate = await _unitOfWork.BookingsRepository.GetById(id, cancellationToken);
        aggregate.Cancel();
        await _bookingsRepository.Update(aggregate, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}