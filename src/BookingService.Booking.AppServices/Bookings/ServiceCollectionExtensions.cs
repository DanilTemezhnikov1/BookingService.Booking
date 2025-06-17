using BookingService.Booking.AppServices.Dates;
using Microsoft.Extensions.DependencyInjection;

namespace BookingService.Booking.AppServices.Bookings
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<IBookingsService, BookingService>();
            services.AddSingleton<ICurrentDateTimeProvider, DefaultCurrentDateTimeProvider>();

            services.Configure<BookingCatalogRestOptions>(configuration.GetSection(nameof(BookingCatalogRestOptions))); 

            services.AddHttpClient(nameof(BookingCatalogRestOptions),
   (ctx, client) =>
   {
       var options = ctx.GetRequiredService<IOptions<BookingCatalogRestOptions>>().Value;
      // client.BaseAddress = new Uri(options.BaseAddress);
       client.Timeout = TimeSpan.FromSeconds(90);
   }).AddTransientHttpErrorPolicy(builder => builder
   .WaitAndRetryAsync(4, retryAttempt => 
   TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

            services.AddScoped<IBookingJobsController>(ctx => RestClient.For<IBookingJobsController>(ctx.GetRequiredService<IHttpClientFactory>()
      .CreateClient(nameof(BookingCatalogRestOptions))));


        }
    }
}
