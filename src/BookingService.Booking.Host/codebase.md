# .vs\BookingService.Booking.Host\DesignTimeBuild\.dtbcache.v2

This is a binary file of the type: Binary

# .vs\BookingService.Booking.Host\FileContentIndex\96144ee0-c559-4e7b-9149-456f377924ba.vsidx

This is a binary file of the type: Binary

# .vs\BookingService.Booking.Host\v17\.futdcache.v2

This is a binary file of the type: Binary

# appsettings.Development.json

```json
{ "Logging": { "LogLevel": { "Default": "Information", "Microsoft.AspNetCore": "Warning" } }, "Serilog": { "MinimumLevel": { "Default": "Information" }, "WriteTo": [ { "Name": "Console", "Args": { "outputTemplate": "[{Level:u3}] {Timestamp:MM-dd HH:mm:ss} {TraceId} {SourceContext:l} {Message:lj}{NewLine}{Exception}" } } ] }, "ConnectionStrings": { "BookingsContext": "server=localhost;port=5433;database=booking_service_bookings;uid=bookings_admin;pwd=admin_bookings" } }
```

# appsettings.json

```json
{ "Logging": { "LogLevel": { "Default": "Information", "Microsoft.AspNetCore": "Warning" } }, "AllowedHosts": "*", "Serilog": { "MinimumLevel": { "Default": "Information", "Override": { "Microsoft": "Information", "Microsoft.AspNetCore": "Error", "System.Net.Http.HttpClient": "Warning" } } }, "BookingJobsCommandHandlerOptions": { "ErrorRandomSeed": null, "ErrorRate": 0.1 }, "CleanUpResourceHandlerOptions": { "ProcessPerCycle": 5, "Interval": "0.00:01:00", "ExceptionInterval": "0.00:05:00" }, "NewBookingJobsProcessorOptions": { "Interval": "0.00:01:00", "ExceptionInterval": "0.00:05:00", "ProcessPerCycle": 5, "ErrorRandomSeed": null, "ErrorRate": 0.1 } }
```

# appsettings.Production.json

```json
{ "Serilog": { "MinimumLevel": { "Default": "Information" }, "WriteTo": [ { "Name": "Console", "Args": { "outputTemplate": "[{Level:u3}] {Timestamp:MM-dd HH:mm:ss} {TraceId} {SourceContext:l} {Message:lj}{NewLine}{Exception}" } }, { "Name": "File", "Args": { "path": "/var/log/booking-service/catalog/booking-service-catalog-.log", "outputTemplate": "[{Level:u3}] {Timestamp:dd-MM-yyyy HH:mm:ss} {TraceId} {SourceContext:l} {Message:lj}{NewLine}{Exception}", "fileSizeLimitBytes": 104857600, "rollingInterval": "Day", "rollOnFileSizeLimit": true } } ] }, "DataOptions": { "ConnectionString": "PORT = 5432; HOST = catalog-db; TIMEOUT = 15; POOLING = True; MINPOOLSIZE = 1; MAXPOOLSIZE = 100; COMMANDTIMEOUT = 20; DATABASE = 'booking_service_catalog'; PASSWORD = 'admin_catalog'; USER ID = 'catalog_admin'" }, "RebusRabbitMqOptions": { "ConnectionString": "amqp://admin:admin@rabbitmq:5672/" }, "ConnectionStrings": { "BookingsContext": "server=localhost;port=5433;database=booking_service_bookings;uid=bookings_admin;pwd=admin_bookings" } }
```

# BookingService.Booking.Host.csproj

```csproj
<Project Sdk="Microsoft.NET.Sdk.Web"> <PropertyGroup> <OutputType>Exe</OutputType> <TargetFramework>net8.0</TargetFramework> <ImplicitUsings>enable</ImplicitUsings> <Nullable>enable</Nullable> <UserSecretsId>ec74bf4a-e47c-4559-a802-64aad15b57eb</UserSecretsId> <DockerDefaultTargetOS>Linux</DockerDefaultTargetOS> </PropertyGroup> <ItemGroup> <PackageReference Include="Hellang.Middleware.ProblemDetails" Version="6.5.1" /> <PackageReference Include="Microsoft.AspNetCore.App" Version="2.2.8" /> <PackageReference Include="Microsoft.VisualStudio.Azure.Containers.Tools.Targets" Version="1.21.0" /> <PackageReference Include="Serilog" Version="4.3.0" /> <PackageReference Include="Serilog.Extensions.Hosting" Version="9.0.0" /> <PackageReference Include="Serilog.Settings.Configuration" Version="9.0.0" /> <PackageReference Include="Serilog.Sinks.Console" Version="6.0.0" /> <PackageReference Include="Serilog.Sinks.File" Version="7.0.0" /> </ItemGroup> <ItemGroup> <ProjectReference Include="..\BookingService.Booking.Api.Contracts\BookingService.Booking.Api.Contracts.csproj" /> <ProjectReference Include="..\BookingService.Booking.AppServices\BookingService.Booking.AppServices.csproj" /> <ProjectReference Include="..\BookingService.Booking.Domain\BookingService.Booking.Domain.csproj" /> <ProjectReference Include="..\BookingService.Booking.Persistence\BookingService.Booking.Persistence.csproj" /> </ItemGroup> <ItemGroup> <PackageReference Include="Swashbuckle.AspNetCore" Version="6.*" /> </ItemGroup> <ItemGroup> <Content Update="appsettings.Development.json"> <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory> <ExcludeFromSingleFile>true</ExcludeFromSingleFile> <CopyToPublishDirectory>PreserveNewest</CopyToPublishDirectory> </Content> <Content Update="appsettings.json"> <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory> <ExcludeFromSingleFile>true</ExcludeFromSingleFile> <CopyToPublishDirectory>PreserveNewest</CopyToPublishDirectory> </Content> <Content Update="appsettings.Production.json"> <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory> <ExcludeFromSingleFile>true</ExcludeFromSingleFile> <CopyToPublishDirectory>PreserveNewest</CopyToPublishDirectory> </Content> </ItemGroup> </Project>
```

# Controllers\BookingsController.cs

```cs
using BookingService.Booking.Api.Contracts.Bookings; using BookingService.Booking.Api.Contracts.Bookings.Requests; using BookingService.Booking.AppServices.Bookings; using BookingService.Booking.AppServices; using BookingService.Booking.Host.Mapping; using Microsoft.AspNetCore.Mvc; namespace BookingService.Booking.Host.Controllers; [ApiController] public class BookingsController : ControllerBase { private readonly IBookingsQueries _bookingsQueries; private readonly IBookingsService _bookingsService; public BookingsController(IBookingsService bookingsService, IBookingsQueries bookingsQueries, CancellationToken cancellationToken) { _bookingsService = bookingsService; _bookingsQueries = bookingsQueries; } [HttpPost] [Route(WebRoutes.Create)] public async Task<long> CreateBooking([FromBody] CreateBookingRequest createBookingRequest, CancellationToken cancellationToken) { return await _bookingsService.Create(createBookingRequest.ToQuery(), cancellationToken); } [HttpGet] [Route(WebRoutes.GetByFilter)] public async Task<BookingData[]> GetBookingsByFilter([FromQuery] GetBookingsByFilterRequest getBookingsByFilter, CancellationToken cancellationToken) { return await _bookingsQueries.GetByFilter(getBookingsByFilter.ToQuery(), cancellationToken); } [HttpPost] [Route(WebRoutes.Cancel)] public async Task CancelBooking([FromBody] long id, CancellationToken cancellationToken) { await _bookingsService.Cancel(id, cancellationToken); } [HttpGet] [Route(WebRoutes.GetById)] public async Task<BookingData> GetBookingById([FromRoute] long id, CancellationToken cancellationToken) { return await _bookingsService.GetById(id, cancellationToken); } [HttpGet] [Route(WebRoutes.GetStatusById)] public async Task<string> GetBookingStatusById([FromRoute] long id, CancellationToken cancellationToken) { return await _bookingsQueries.GetStatusById(id, cancellationToken); } }
```

# Dockerfile

```
# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки. # Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки) FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base USER $APP_UID WORKDIR /app EXPOSE 8080 EXPOSE 8081 # Этот этап используется для сборки проекта службы FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build ARG BUILD_CONFIGURATION=Release WORKDIR /src COPY ["src/BookingService.Booking.Host/BookingService.Booking.Host.csproj", "BookingService.Booking.Host/"] COPY ["src/BookingService.Booking.Api.Contracts/BookingService.Booking.Api.Contracts.csproj", "BookingService.Booking.Api.Contracts/"] COPY ["src/BookingService.Booking.AppServices/BookingService.Booking.AppServices.csproj", "BookingService.Booking.AppServices/"] RUN dotnet restore "./BookingService.Booking.Host/BookingService.Booking.Host.csproj" COPY . . WORKDIR "/src/BookingService.Booking.Host" RUN dotnet build "src/BookingService.Booking.Host/BookingService.Booking.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build # Этот этап используется для публикации проекта службы, который будет скопирован на последний этап FROM build AS publish ARG BUILD_CONFIGURATION=Release RUN dotnet publish "./BookingService.Booking.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false # Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется) FROM base AS final WORKDIR /app COPY --from=publish /app/publish . ENTRYPOINT ["dotnet", "BookingService.Booking.Host.dll"]
```

# HostBuilderFactory.cs

```cs
using Serilog; namespace BookingService.Booking.Host; public static class HostBuilderFactory { public static IHost BuildHost(string[] args) { return Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args) .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); }) .UseSerilog() // Подключение Serilog .Build(); } }
```

# Mapping\BookingMappings.cs

```cs
using BookingService.Booking.Api.Contracts.Bookings.Requests; using BookingService.Booking.AppServices.Queries; namespace BookingService.Booking.Host.Mapping; public static class BookingMappings { public static CreateBookingQuery ToQuery(this CreateBookingRequest request) { return new CreateBookingQuery { IdUser = request.IdUser, IdBooking = request.IdBooking, StartBooking = request.StartBooking, EndBooking = request.EndBooking }; } public static GetBookingsByFilterQuery ToQuery(this GetBookingsByFilterRequest request) { return new GetBookingsByFilterQuery { Id = request.Id, IdUser = request.IdUser, IdBooking = request.IdBooking, Status = request.Status, CreationBooking = request.CreationBooking, StartBooking = request.StartBooking, EndBooking = request.EndBooking }; } }
```

# Program.cs

```cs
//var builder = WebApplication.CreateBuilder(args); //var app = builder.Build(); //app.MapGet("/", () => "Hello World!"); //app.Run(); using BookingService.Booking.Host; var host = HostBuilderFactory.BuildHost(args); host.Run();
```

# Properties\launchSettings.json

```json
{ "profiles": { "BookingService.Booking.Host": { "commandName": "Project", "launchBrowser": true, "environmentVariables": { "ASPNETCORE_ENVIRONMENT": "Development" }, "applicationUrl": "https://localhost:63915;http://localhost:63916" }, "Container (Dockerfile)": { "commandName": "Docker", "launchBrowser": true, "launchUrl": "{Scheme}://{ServiceHost}:{ServicePort}", "environmentVariables": { "ASPNETCORE_HTTPS_PORTS": "8081", "ASPNETCORE_HTTP_PORTS": "8080" }, "publishAllPorts": true, "useSSL": true } } }
```

# Startup.cs

```cs
using BookingService.Booking.AppServices.Bookings; using BookingService.Booking.AppServices.Exceptions; using BookingService.Booking.Domain.Exceptions; using BookingService.Booking.Persistence; using Hellang.Middleware.ProblemDetails; using Microsoft.AspNetCore.Mvc; using Microsoft.OpenApi.Models; namespace BookingService.Booking.Host; public class Startup { private readonly IConfiguration _configuration; public Startup(IConfiguration configuration) { _configuration = configuration; } // Настройка сервисов public void ConfigureServices(IServiceCollection services) { // Регистрация сервисов в DI-контейнере services.AddControllers(); // Добавление Swagger services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Booking Service API", Version = "v1", Description = "API для сервиса бронирования" }); }); services.AddAppServices(); services.AddPersistence(_configuration.GetConnectionString("BookingsContext")); services.AddProblemDetails(options => { // Если окружение Development, включаем подробное описание ошибки в ответ. options.IncludeExceptionDetails = (context, _) => { var env = context.RequestServices.GetRequiredService<IWebHostEnvironment>(); return env.IsDevelopment(); }; options.Map<DomainException>(ex => new ProblemDetails { Status = 402, Type = $"https://httpstatuses.com/{402}", Title = ex.Message, Detail = ex.StackTrace }); options.Map<ValidationException>(ex => new ProblemDetails { Status = 400, Type = $"https://httpstatuses.com/{400}", Title = ex.Message }); }); } // Настройка middleware public void Configure(IApplicationBuilder app, IWebHostEnvironment env) { if (env.IsDevelopment()) { app.UseDeveloperExceptionPage(); app.UseSwagger(); app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Booking Service API V1"); }); } app.UseRouting(); app.UseEndpoints(endpoints => { endpoints.MapControllers(); }); app.UseProblemDetails(); } }
```

