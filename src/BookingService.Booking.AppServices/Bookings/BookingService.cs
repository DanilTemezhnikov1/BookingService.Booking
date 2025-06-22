using BookingService.Booking.AppServices.Dates;
using BookingService.Booking.AppServices.Queries;
using BookingService.Booking.Domain.Bookings;
using BookingService.Catalog.Api.Contracts.BookingJobs;
using BookingService.Catalog.Api.Contracts.BookingJobs.Commands;

namespace BookingService.Booking.AppServices.Bookings
{
    internal class BookingService : IBookingsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBookingsRepository _bookingsRepository;
        private readonly ICurrentDateTimeProvider _currentDateTimeProvider;
        private readonly IBookingJobsController _bookingJobsController;

        public BookingService(IUnitOfWork unitOfWork, ICurrentDateTimeProvider timeProvider, IBookingJobsController bookingJobsController)
        {
            _unitOfWork = unitOfWork;
            _bookingsRepository = _unitOfWork.BookingsRepository;
            _currentDateTimeProvider = timeProvider;
            _bookingJobsController = bookingJobsController;
        }

        public async Task<long> Create(CreateBookingQuery createBooking, CancellationToken cancellationToken)
        {
            var aggregate = BookingAggregate.Initialize(
                createBooking.IdUser,
                createBooking.IdBooking,
                createBooking.StartBooking,
                createBooking.EndBooking,
                _currentDateTimeProvider.UtcNow);
            aggregate.SetCatalogRequestId(Guid.NewGuid());
            await _bookingsRepository.Create(aggregate, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            await _bookingJobsController.CreateBookingJob(aggregate.ToCreateBookingJobCommand());
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
            if (aggregate.CatalogRequestId != null)
                await _bookingJobsController.CancelBookingJob(
                    new CancelBookingJobByRequestIdCommand
                    {
                        RequestId = aggregate.CatalogRequestId.Value
                    }, 
                    cancellationToken);
        }
    }
}