using BookingService.Booking.AppServices.Dates;
using BookingService.Booking.AppServices.Queries;
using BookingService.Booking.Domain;
using BookingService.Booking.Domain.Bookings;
using BookingService.Catalog.Api.Contracts.BookingJobs;
using BookingService.Catalog.Api.Contracts.BookingJobs.Commands;
using BookingService.Catalog.Api.Contracts.BookingJobs.Queries;
using RestEase;

namespace BookingService.Booking.AppServices.Bookings
{
    internal class BookingService : IBookingsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBookingsRepository _bookingsRepository;
        private readonly ICurrentDateTimeProvider _currentDateTimeProvider;
        private readonly IBookingJobsController _vbookingJobsController;

        public BookingService(IUnitOfWork unitOfWork, ICurrentDateTimeProvider timeProvider, IBookingJobsController vbookingJobsController)
        {
            _unitOfWork = unitOfWork;
            _bookingsRepository = _unitOfWork.BookingsRepository;
            _currentDateTimeProvider = timeProvider;
            _vbookingJobsController = vbookingJobsController;
        }

        public async Task<long> Create(CreateBookingQuery createBooking)
        {
            var aggregate = BookingAggregate.Initialize(
                createBooking.IdUser,
                createBooking.IdBooking,
                createBooking.StartBooking,
                createBooking.EndBooking,
                _currentDateTimeProvider.Now);
            aggregate.SetCatalogRequestId(Guid.NewGuid());
            _bookingsRepository.Create(aggregate);          
            await _unitOfWork.CommitAsync();
            await _vbookingJobsController.CreateBookingJob(aggregate.ToCreateBookingJobCommand());
            return aggregate.Id;
        }
        public async Task<BookingData> GetById(long id)
        {
            return _bookingsRepository.GetById(id).Result.ToBookingData();
        }
        public async Task Cancel(long id)
        {
            var aggregate = _unitOfWork.BookingsRepository.GetById(id).Result;
            aggregate.Cancel();
            _bookingsRepository.Update(aggregate);
            await _unitOfWork.CommitAsync();
        }
    }
}
