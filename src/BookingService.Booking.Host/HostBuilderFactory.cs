using Serilog;

namespace BookingService.Booking.Host;

public static class HostBuilderFactory
{
    public static IHost BuildHost(string[] args)
    {
        return Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
            .UseSerilog() // Подключение Serilog
            .Build();
    }
}