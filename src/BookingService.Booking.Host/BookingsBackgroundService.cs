using BookingService.Booking.AppServices.Bookings.Jobs;

namespace BookingService.Booking.Host
{
    public class BookingsBackgroundService : BackgroundService
    {
        private IServiceProvider _serviceProvider;
        private ILogger _logger;
        public BookingsBackgroundService(IServiceProvider serviceProvider, ILogger logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var scope = _serviceProvider.CreateScope();
                    var backgroundServiceHandler = scope.ServiceProvider.GetRequiredService<IBookingsBackgroundServiceHandler>();
                    await backgroundServiceHandler.Handle(stoppingToken);
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
                catch
                {
                    _logger.LogError("Не удалось получить экземляр сервиса");
                };
            }
        }
    }
}
