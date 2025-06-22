using BookingService.Booking.AppServices.Bookings.Jobs;
using BookingService.Booking.AppServices.Exceptions;
using BookingService.Booking.Domain.Bookings;
using BookingService.Catalog.Api.Contracts.BookingJobs;
using BookingService.Catalog.Api.Contracts.BookingJobs.Queries;
using Microsoft.Extensions.Logging;

namespace BookingService.Booking.AppServices
{
    public class BookingsBackgroundServiceHandler : IBookingsBackgroundServiceHandler
    {
        private readonly IBookingsBackgroundQueries _bookingsBackgroundQueries;
        private readonly IBookingJobsController _bookingJobsController;
        private readonly ILogger<BookingsBackgroundServiceHandler> _logger; 
        private readonly IBookingsRepository _bookingsRepository;
        private readonly IUnitOfWork _unitOfWork;
        public BookingsBackgroundServiceHandler(IBookingsBackgroundQueries bookingsBackgroundQueries, 
            IBookingJobsController bookingJobsController, 
            ILogger<BookingsBackgroundServiceHandler> logger, 
            IUnitOfWork unitOfWork)
        {
            _bookingsBackgroundQueries = bookingsBackgroundQueries;
            _bookingJobsController = bookingJobsController;
            _logger = logger;
            _bookingsRepository = unitOfWork.BookingsRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(CancellationToken cancellationToken)
        {
            var bookingAggregates = _bookingsBackgroundQueries.GetConfirmationAwaitingBookings(10);
            foreach (var bookingAggregate in bookingAggregates)
            {
                _logger.LogWarning("У агрегата {0} некорректное состояние", bookingAggregate.Id);
                var jobStatus = await _bookingJobsController.GetBookingJobStatusByRequestId(
                     new GetBookingJobStatusByRequestIdQuery
                     { RequestId = bookingAggregate.CatalogRequestId.Value });
                switch (jobStatus)
                {
                    case BookingJobStatus.Confirmed: bookingAggregate.Confirm(); break;
                    case BookingJobStatus.Cancelled: bookingAggregate.Cancel(); break;
                   // default: throw new ValidationException("Некорректное состояние");
                };
                await _bookingsRepository.Update(bookingAggregate, cancellationToken);
                await _unitOfWork.CommitAsync(cancellationToken);
            }
        }
    }
}
