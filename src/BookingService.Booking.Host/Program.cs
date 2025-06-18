//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();
//app.MapGet("/", () => "Hello World!");
//app.Run();

using BookingService.Booking.Host;

var host = HostBuilderFactory.BuildHost(args);
host.Run();