var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
// sffsff
app.MapGet("/", () => "Hello World!");
app.Run();
