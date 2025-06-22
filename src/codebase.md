# BookingService.Booking.Api.Contracts\Bookings\Dtos\BookingDataResponse.cs

```cs
using BookingService.Booking.Domain.Contracts.Bookings; namespace BookingService.Booking.Api.Contracts.Bookings.Dtos; public class BookingDataResponse { public DateTimeOffset CreationBooking; public DateOnly EndBooking; public long Id; public long IdBooking; public long IdUser; public DateOnly StartBooking; public BookingStatus Status; }
```

# BookingService.Booking.Api.Contracts\Bookings\Requests\CreateBookingRequest.cs

```cs
namespace BookingService.Booking.Api.Contracts.Bookings.Requests; public record CreateBookingRequest( long IdUser, long IdBooking, DateOnly StartBooking, DateOnly EndBooking);
```

# BookingService.Booking.Api.Contracts\Bookings\Requests\GetBookingsByFilterRequest.cs

```cs
using BookingService.Booking.Domain.Contracts.Bookings; namespace BookingService.Booking.Api.Contracts.Bookings.Requests; public record GetBookingsByFilterRequest( long? Id, BookingStatus? Status, long? IdUser, long? IdBooking, DateOnly? StartBooking, DateOnly? EndBooking, DateTimeOffset? CreationBooking);
```

# BookingService.Booking.Api.Contracts\Bookings\WebRoutes.cs

```cs
namespace BookingService.Booking.Api.Contracts.Bookings; public static class WebRoutes { public const string BasePath = "api/bookings"; public const string Create = BasePath + "/create"; public const string GetById = BasePath + "/{id}"; public const string Cancel = BasePath + "/{id}/cancel"; public const string GetByFilter = BasePath + "/by-filter"; public const string GetStatusById = BasePath + "/{id}/status"; }
```

# BookingService.Booking.Api.Contracts\BookingService.Booking.Api.Contracts.csproj

```csproj
<Project Sdk="Microsoft.NET.Sdk"> <PropertyGroup> <TargetFramework>net8.0</TargetFramework> <ImplicitUsings>enable</ImplicitUsings> <Nullable>enable</Nullable> </PropertyGroup> <ItemGroup> <ProjectReference Include="..\BookingService.Booking.Domain.Contracts\BookingService.Booking.Domain.Contracts.csproj" /> </ItemGroup> </Project>
```

# BookingService.Booking.AppServices.Contracts\Bookings\IBookingsQueries.cs

```cs
using BookingService.Booking.AppServices.Bookings; using BookingService.Booking.AppServices.Queries; namespace BookingService.Booking.AppServices.Contracts; public interface IBookingsQueries { Task<BookingData[]> GetByFilter(GetBookingsByFilterQuery getBookingsByFilter, CancellationToken cancellationToken); Task<string> GetStatusById(long id, CancellationToken cancellationToken); }
```

# BookingService.Booking.AppServices.Contracts\BookingService.Booking.AppServices.Contracts.csproj

```csproj
<Project Sdk="Microsoft.NET.Sdk"> <PropertyGroup> <TargetFramework>net8.0</TargetFramework> <ImplicitUsings>enable</ImplicitUsings> <Nullable>enable</Nullable> </PropertyGroup> <ItemGroup> <ProjectReference Include="..\BookingService.Booking.AppServices\BookingService.Booking.AppServices.csproj" /> </ItemGroup> </Project>
```

# BookingService.Booking.AppServices.Contracts\Class1.cs

```cs
namespace BookingService.Booking.AppServices.Contracts; public class Class1 { }
```

# BookingService.Booking.AppServices\BookingAggregateExtension.cs

```cs
using BookingService.Booking.AppServices.Bookings; using BookingService.Booking.Domain.Bookings; using BookingService.Catalog.Api.Contracts.BookingJobs.Commands; namespace BookingService.Booking.AppServices { public static class BookingAggregateExtension { public static BookingData ToBookingData(this BookingAggregate? aggregate) { return new BookingData { Id = aggregate.Id, Status = aggregate.Status, IdUser = aggregate.IdUser, IdBooking = aggregate.IdBooking, StartBooking = aggregate.StartBooking, EndBooking = aggregate.EndBooking, CreationBooking = aggregate.CreationBooking }; } public static CreateBookingJobCommand ToCreateBookingJobCommand(this BookingAggregate aggregate) { return new CreateBookingJobCommand { RequestId = aggregate.CatalogRequestId.Value, ResourceId = aggregate.Id, StartDate = aggregate.StartBooking, EndDate = aggregate.EndBooking, }; } } }
```

# BookingService.Booking.AppServices\Bookings\BookingData.cs

```cs
using BookingService.Booking.Domain.Contracts.Bookings; namespace BookingService.Booking.AppServices.Bookings; public class BookingData { public DateTimeOffset CreationBooking; public DateOnly EndBooking; public long Id; public long IdBooking; public long IdUser; public DateOnly StartBooking; public BookingStatus Status; }
```

# BookingService.Booking.AppServices\Bookings\BookingService.cs

```cs
using BookingService.Booking.AppServices.Dates; using BookingService.Booking.AppServices.Queries; using BookingService.Booking.Domain.Bookings; using BookingService.Catalog.Api.Contracts.BookingJobs; using BookingService.Catalog.Api.Contracts.BookingJobs.Commands; namespace BookingService.Booking.AppServices.Bookings { internal class BookingService : IBookingsService { private readonly IUnitOfWork _unitOfWork; private readonly IBookingsRepository _bookingsRepository; private readonly ICurrentDateTimeProvider _currentDateTimeProvider; private readonly IBookingJobsController _bookingJobsController; public BookingService(IUnitOfWork unitOfWork, ICurrentDateTimeProvider timeProvider, IBookingJobsController bookingJobsController) { _unitOfWork = unitOfWork; _bookingsRepository = _unitOfWork.BookingsRepository; _currentDateTimeProvider = timeProvider; _bookingJobsController = bookingJobsController; } public async Task<long> Create(CreateBookingQuery createBooking, CancellationToken cancellationToken) { var aggregate = BookingAggregate.Initialize( createBooking.IdUser, createBooking.IdBooking, createBooking.StartBooking, createBooking.EndBooking, _currentDateTimeProvider.UtcNow); aggregate.SetCatalogRequestId(Guid.NewGuid()); await _bookingsRepository.Create(aggregate, cancellationToken); await _unitOfWork.CommitAsync(cancellationToken); await _bookingJobsController.CreateBookingJob(aggregate.ToCreateBookingJobCommand()); return aggregate.Id; } public async Task<BookingData> GetById(long id, CancellationToken cancellationToken) { var aggregate = await _bookingsRepository.GetById(id, cancellationToken); return aggregate.ToBookingData(); } public async Task Cancel(long id, CancellationToken cancellationToken) { var aggregate = await _unitOfWork.BookingsRepository.GetById(id, cancellationToken); aggregate.Cancel(); await _bookingsRepository.Update(aggregate, cancellationToken); await _unitOfWork.CommitAsync(cancellationToken); if (aggregate.CatalogRequestId != null) await _bookingJobsController.CancelBookingJob( new CancelBookingJobByRequestIdCommand { RequestId = aggregate.CatalogRequestId.Value }, cancellationToken); } } }
```

# BookingService.Booking.AppServices\Bookings\IBookingsService.cs

```cs
using BookingService.Booking.AppServices.Queries; namespace BookingService.Booking.AppServices.Bookings; public interface IBookingsService { Task<long> Create(CreateBookingQuery createBooking, CancellationToken cancellationToken); Task<BookingData> GetById(long id, CancellationToken cancellationToken); Task Cancel(long id , CancellationToken cancellationToken); }
```

# BookingService.Booking.AppServices\Bookings\Jobs\IBookingsBackgroundServiceHandler.cs

```cs
namespace BookingService.Booking.AppServices.Bookings.Jobs { public interface IBookingsBackgroundServiceHandler { public Task Handle(CancellationToken cancellationToken); } }
```

# BookingService.Booking.AppServices\Bookings\ServiceCollectionExtensions.cs

```cs
using BookingService.Booking.AppServices.Dates; using BookingService.Booking.AppServices.Options; using BookingService.Catalog.Api.Contracts.BookingJobs; using Microsoft.Extensions.Configuration; using Microsoft.Extensions.DependencyInjection; using Microsoft.Extensions.Options; using RestEase; using Polly; namespace BookingService.Booking.AppServices.Bookings { public static class ServiceCollectionExtensions { public static void AddAppServices(this IServiceCollection services, IConfiguration configuration) { services.AddScoped<IBookingsService, BookingService>(); services.AddSingleton<ICurrentDateTimeProvider, DefaultCurrentDateTimeProvider>(); services.Configure<BookingCatalogRestOptions>(configuration.GetSection(nameof(BookingCatalogRestOptions))); services.AddHttpClient(nameof(BookingCatalogRestOptions), (ctx, client) => { var options = ctx.GetRequiredService<IOptions<BookingCatalogRestOptions>>().Value; client.BaseAddress = new Uri(options.BaseAddress); client.Timeout = TimeSpan.FromSeconds(90); }).AddTransientHttpErrorPolicy(builder => builder .WaitAndRetryAsync(4, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))); services.AddScoped<IBookingJobsController>(ctx => RestClient.For<IBookingJobsController>(ctx.GetRequiredService<IHttpClientFactory>() .CreateClient(nameof(BookingCatalogRestOptions)))); } } }
```

# BookingService.Booking.AppServices\BookingsBackgroundServiceHandler.cs

```cs
using BookingService.Booking.AppServices.Bookings.Jobs; using BookingService.Booking.AppServices.Exceptions; using BookingService.Booking.Domain.Bookings; using BookingService.Catalog.Api.Contracts.BookingJobs; using BookingService.Catalog.Api.Contracts.BookingJobs.Queries; using Microsoft.Extensions.Logging; namespace BookingService.Booking.AppServices { public class BookingsBackgroundServiceHandler : IBookingsBackgroundServiceHandler { private readonly IBookingsBackgroundQueries _bookingsBackgroundQueries; private readonly IBookingJobsController _bookingJobsController; private readonly ILogger<BookingsBackgroundServiceHandler> _logger; private readonly IBookingsRepository _bookingsRepository; private readonly IUnitOfWork _unitOfWork; public BookingsBackgroundServiceHandler(IBookingsBackgroundQueries bookingsBackgroundQueries, IBookingJobsController bookingJobsController, ILogger<BookingsBackgroundServiceHandler> logger, IUnitOfWork unitOfWork) { _bookingsBackgroundQueries = bookingsBackgroundQueries; _bookingJobsController = bookingJobsController; _logger = logger; _bookingsRepository = unitOfWork.BookingsRepository; _unitOfWork = unitOfWork; } public async Task Handle(CancellationToken cancellationToken) { var bookingAggregates = _bookingsBackgroundQueries.GetConfirmationAwaitingBookings(10); foreach (var bookingAggregate in bookingAggregates) { _logger.LogWarning("У агрегата {0} некорректное состояние", bookingAggregate.Id); var jobStatus = await _bookingJobsController.GetBookingJobStatusByRequestId( new GetBookingJobStatusByRequestIdQuery { RequestId = bookingAggregate.CatalogRequestId.Value }); switch (jobStatus) { case BookingJobStatus.Confirmed: bookingAggregate.Confirm(); break; case BookingJobStatus.Cancelled: bookingAggregate.Cancel(); break; // default: throw new ValidationException("Некорректное состояние"); }; await _bookingsRepository.Update(bookingAggregate, cancellationToken); await _unitOfWork.CommitAsync(cancellationToken); } } } }
```

# BookingService.Booking.AppServices\BookingService.Booking.AppServices.csproj

```csproj
<Project Sdk="Microsoft.NET.Sdk"> <PropertyGroup> <TargetFramework>net8.0</TargetFramework> <ImplicitUsings>enable</ImplicitUsings> <Nullable>enable</Nullable> </PropertyGroup> <ItemGroup> <PackageReference Include="BookingService.Catalog.Api.Contracts" Version="2.1.2" /> <PackageReference Include="Microsoft.Extensions.DependencyInjection.Abstractions" Version="8.0.0" /> <PackageReference Include="Microsoft.Extensions.Diagnostics.Abstractions" Version="8.0.0" /> <PackageReference Include="Microsoft.Extensions.Hosting.Abstractions" Version="8.0.0" /> <PackageReference Include="Microsoft.Extensions.Http" Version="8.0.0" /> <PackageReference Include="Microsoft.Extensions.Http.Polly" Version="8.0.0" /> <PackageReference Include="Microsoft.Extensions.Options.ConfigurationExtensions" Version="8.0.0" /> </ItemGroup> <ItemGroup> <ProjectReference Include="..\BookingService.Booking.Domain.Contracts\BookingService.Booking.Domain.Contracts.csproj" /> <ProjectReference Include="..\BookingService.Booking.Domain\BookingService.Booking.Domain.csproj" /> </ItemGroup> </Project>
```

# BookingService.Booking.AppServices\Dates\DefaultCurrentDateTimeProvider.cs

```cs
namespace BookingService.Booking.AppServices.Dates; public class DefaultCurrentDateTimeProvider : ICurrentDateTimeProvider { public DateTimeOffset Now => DateTimeOffset.Now.ToLocalTime(); public DateTimeOffset UtcNow => DateTimeOffset.UtcNow; }
```

# BookingService.Booking.AppServices\Dates\ICurrentDateTimeProvider.cs

```cs
namespace BookingService.Booking.AppServices.Dates; public interface ICurrentDateTimeProvider { public DateTimeOffset Now { get; } public DateTimeOffset UtcNow { get; } }
```

# BookingService.Booking.AppServices\Exceptions\ValidationException.cs

```cs
namespace BookingService.Booking.AppServices.Exceptions; public class ValidationException : Exception { public ValidationException() { } public ValidationException(string str) : base(str) { } }
```

# BookingService.Booking.AppServices\IBookingsQueries.cs

```cs
using BookingService.Booking.AppServices.Bookings; using BookingService.Booking.AppServices.Queries; namespace BookingService.Booking.AppServices; public interface IBookingsQueries { Task<BookingData[]> GetByFilter(GetBookingsByFilterQuery getBookingsByFilter, CancellationToken cancellationToken); Task<string> GetStatusById(long id, CancellationToken cancellationToken); }
```

# BookingService.Booking.AppServices\IBookingsRepository.cs

```cs
using BookingService.Booking.Domain.Bookings; namespace BookingService.Booking.AppServices; public interface IBookingsRepository { public Task Create(BookingAggregate aggregate, CancellationToken cancellationToken); public ValueTask<BookingAggregate?> GetById(long id, CancellationToken token = default); public Task Update(BookingAggregate aggregate, CancellationToken cancellationToken); }
```

# BookingService.Booking.AppServices\IUnitOfWork.cs

```cs
namespace BookingService.Booking.AppServices; public interface IUnitOfWork { public IBookingsRepository BookingsRepository { get; } Task CommitAsync(CancellationToken cancellationToken = default); }
```

# BookingService.Booking.AppServices\Options\BookingCatalogRestOptions.cs

```cs
namespace BookingService.Booking.AppServices.Options; public class BookingCatalogRestOptions { public string BaseAddress { get; set; } }
```

# BookingService.Booking.AppServices\Queries\CreateBookingQuery.cs

```cs
namespace BookingService.Booking.AppServices.Queries; public class CreateBookingQuery { public long IdUser { get; set; } public long IdBooking { get; set; } public DateOnly StartBooking { get; set; } public DateOnly EndBooking { get; set; } }
```

# BookingService.Booking.AppServices\Queries\GetBookingsByFilterQuery.cs

```cs
using BookingService.Booking.Domain.Contracts.Bookings; namespace BookingService.Booking.AppServices.Queries; public class GetBookingsByFilterQuery { public long? Id { get; set; } public BookingStatus? Status { get; set; } public long? IdUser { get; set; } public long? IdBooking { get; set; } public DateOnly? StartBooking { get; set; } public DateOnly? EndBooking { get; set; } public DateTimeOffset? CreationBooking { get; set; } }
```

# BookingService.Booking.Domain.Contracts\Bookings\EnumBookingData.cs

```cs
namespace BookingService.Booking.Domain.Contracts.Bookings; public enum BookingStatus { AwaitsConfirmation, Confirmed, Cancelled }
```

# BookingService.Booking.Domain.Contracts\BookingService.Booking.Domain.Contracts.csproj

```csproj
<Project Sdk="Microsoft.NET.Sdk"> <PropertyGroup> <TargetFramework>net8.0</TargetFramework> <ImplicitUsings>enable</ImplicitUsings> <Nullable>enable</Nullable> </PropertyGroup> </Project>
```

# BookingService.Booking.Domain\Bookings\BookingAggregate.cs

```cs
using BookingService.Booking.Domain.Contracts.Bookings; using BookingService.Booking.Domain.Exceptions; namespace BookingService.Booking.Domain.Bookings { public class BookingAggregate { public Guid? CatalogRequestId { get; private set; } public long Id { get; set; } public BookingStatus Status { get; private set; } public long IdUser { get; private set; } public long IdBooking { get; private set; } public DateOnly StartBooking { get; private set; } public DateOnly EndBooking { get; private set; } public DateTimeOffset CreationBooking { get; private set; } public BookingAggregate() { } private BookingAggregate(long idUser, long idBooking, DateOnly startBooking, DateOnly endBooking, DateTimeOffset creationBooking) { Status = BookingStatus.AwaitsConfirmation; IdUser = idUser; IdBooking = idBooking; StartBooking = startBooking; EndBooking = endBooking; CreationBooking = creationBooking; } public void SetCatalogRequestId(Guid catalogRequestId) { if (catalogRequestId == default) throw new DomainException(); if (CatalogRequestId == null) CatalogRequestId = catalogRequestId; else throw new DomainException(); } public static BookingAggregate Initialize(long idUser, long idBooking, DateOnly startBooking, DateOnly endBooking, DateTimeOffset creationBooking) { if (idUser <= 0 || idBooking <= 0) throw new DomainException("Id должен быть больше нуля"); if (startBooking.CompareTo(DateOnly.FromDateTime(creationBooking.DateTime)) <= 0) throw new DomainException("Начало бронирования должно быть после текущей даты"); if (endBooking.CompareTo(startBooking) <= 0) throw new DomainException("Окончание бронирования должно быть после начала бронирования"); return new BookingAggregate(idUser, idBooking, startBooking, endBooking, creationBooking); } public void Confirm() { if (Status == BookingStatus.Confirmed) throw new DomainException("Статус уже подтверждён"); if (Status == BookingStatus.Cancelled) throw new DomainException("Статус уже отменён"); if (Status == BookingStatus.AwaitsConfirmation) Status = BookingStatus.Confirmed; } public void Cancel() { if (Status == BookingStatus.Cancelled) throw new DomainException("Статус уже отменён"); Status = BookingStatus.Cancelled; } } }
```

# BookingService.Booking.Domain\Bookings\IBookingsBackgroundQueries.cs

```cs
using System; using System.Collections.Generic; using System.Linq; using System.Text; using System.Threading.Tasks; namespace BookingService.Booking.Domain.Bookings { public interface IBookingsBackgroundQueries { public IReadOnlyCollection<BookingAggregate> GetConfirmationAwaitingBookings(int countBookings = 10); } }
```

# BookingService.Booking.Domain\BookingService.Booking.Domain.csproj

```csproj
<Project Sdk="Microsoft.NET.Sdk"> <PropertyGroup> <TargetFramework>net8.0</TargetFramework> <ImplicitUsings>enable</ImplicitUsings> <Nullable>enable</Nullable> </PropertyGroup> <ItemGroup> <ProjectReference Include="..\BookingService.Booking.Domain.Contracts\BookingService.Booking.Domain.Contracts.csproj" /> </ItemGroup> </Project>
```

# BookingService.Booking.Domain\Exceptions\DomainException.cs

```cs
namespace BookingService.Booking.Domain.Exceptions; public class DomainException : Exception { public DomainException() { } public DomainException(string str) : base(str) { } }
```

# BookingService.Booking.Host\.vs\BookingService.Booking.Host\DesignTimeBuild\.dtbcache.v2

This is a binary file of the type: Binary

# BookingService.Booking.Host\.vs\BookingService.Booking.Host\FileContentIndex\96144ee0-c559-4e7b-9149-456f377924ba.vsidx

This is a binary file of the type: Binary

# BookingService.Booking.Host\.vs\BookingService.Booking.Host\v17\.futdcache.v2

This is a binary file of the type: Binary

# BookingService.Booking.Host\appsettings.Development.json

```json
{ "Logging": { "LogLevel": { "Default": "Information", "Microsoft.AspNetCore": "Warning" } }, "Serilog": { "MinimumLevel": { "Default": "Information" }, "WriteTo": [ { "Name": "Console", "Args": { "outputTemplate": "[{Level:u3}] {Timestamp:MM-dd HH:mm:ss} {TraceId} {SourceContext:l} {Message:lj}{NewLine}{Exception}" } } ] }, "ConnectionStrings": { "BookingsContext": "server=localhost;port=5433;database=booking_service_bookings;uid=bookings_admin;pwd=admin_bookings" }, "BookingCatalogRestOptions": { "BaseAddress": "http://localhost:8000" } }
```

# BookingService.Booking.Host\appsettings.json

```json
{ "Logging": { "LogLevel": { "Default": "Information", "Microsoft.AspNetCore": "Warning" } }, "AllowedHosts": "*", "Serilog": { "MinimumLevel": { "Default": "Information", "Override": { "Microsoft": "Information", "Microsoft.AspNetCore": "Error", "System.Net.Http.HttpClient": "Warning" } } }, "BookingJobsCommandHandlerOptions": { "ErrorRandomSeed": null, "ErrorRate": 0.1 }, "CleanUpResourceHandlerOptions": { "ProcessPerCycle": 5, "Interval": "0.00:01:00", "ExceptionInterval": "0.00:05:00" }, "NewBookingJobsProcessorOptions": { "Interval": "0.00:01:00", "ExceptionInterval": "0.00:05:00", "ProcessPerCycle": 5, "ErrorRandomSeed": null, "ErrorRate": 0.1 } }
```

# BookingService.Booking.Host\appsettings.Production.json

```json
{ "Serilog": { "MinimumLevel": { "Default": "Information" }, "WriteTo": [ { "Name": "Console", "Args": { "outputTemplate": "[{Level:u3}] {Timestamp:MM-dd HH:mm:ss} {TraceId} {SourceContext:l} {Message:lj}{NewLine}{Exception}" } }, { "Name": "File", "Args": { "path": "/var/log/booking-service/catalog/booking-service-catalog-.log", "outputTemplate": "[{Level:u3}] {Timestamp:dd-MM-yyyy HH:mm:ss} {TraceId} {SourceContext:l} {Message:lj}{NewLine}{Exception}", "fileSizeLimitBytes": 104857600, "rollingInterval": "Day", "rollOnFileSizeLimit": true } } ] }, "DataOptions": { "ConnectionString": "PORT = 5432; HOST = catalog-db; TIMEOUT = 15; POOLING = True; MINPOOLSIZE = 1; MAXPOOLSIZE = 100; COMMANDTIMEOUT = 20; DATABASE = 'booking_service_catalog'; PASSWORD = 'admin_catalog'; USER ID = 'catalog_admin'" }, "RebusRabbitMqOptions": { "ConnectionString": "amqp://admin:admin@rabbitmq:5672/" }, "ConnectionStrings": { "BookingsContext": "server=localhost;port=5433;database=booking_service_bookings;uid=bookings_admin;pwd=admin_bookings" }, "BookingCatalogRestOptions": { "BaseAddress": "http://booking-service_catalog-host:8080" } }
```

# BookingService.Booking.Host\BookingsBackgroundService.cs

```cs
using BookingService.Booking.AppServices.Bookings.Jobs; namespace BookingService.Booking.Host { public class BookingsBackgroundService : BackgroundService { private IServiceProvider _serviceProvider; private ILogger<BookingsBackgroundService> _logger; public BookingsBackgroundService(IServiceProvider serviceProvider, ILogger<BookingsBackgroundService> logger) { _serviceProvider = serviceProvider; _logger = logger; } protected override async Task ExecuteAsync(CancellationToken stoppingToken) { while (!stoppingToken.IsCancellationRequested) { try { var scope = _serviceProvider.CreateScope(); var backgroundServiceHandler = scope.ServiceProvider.GetRequiredService<IBookingsBackgroundServiceHandler>(); await backgroundServiceHandler.Handle(stoppingToken); await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken); } catch { _logger.LogError("Не удалось получить экземляр сервиса"); }; } } } }
```

# BookingService.Booking.Host\BookingService.Booking.Host.csproj

```csproj
<Project Sdk="Microsoft.NET.Sdk.Web"> <PropertyGroup> <OutputType>Exe</OutputType> <TargetFramework>net8.0</TargetFramework> <ImplicitUsings>enable</ImplicitUsings> <Nullable>enable</Nullable> <UserSecretsId>ec74bf4a-e47c-4559-a802-64aad15b57eb</UserSecretsId> <DockerDefaultTargetOS>Linux</DockerDefaultTargetOS> </PropertyGroup> <ItemGroup> <PackageReference Include="Hellang.Middleware.ProblemDetails" Version="6.5.1" /> <PackageReference Include="Microsoft.AspNetCore.App" Version="2.2.8" /> <PackageReference Include="Microsoft.VisualStudio.Azure.Containers.Tools.Targets" Version="1.21.0" /> <PackageReference Include="Serilog" Version="4.3.0" /> <PackageReference Include="Serilog.Extensions.Hosting" Version="9.0.0" /> <PackageReference Include="Serilog.Settings.Configuration" Version="9.0.0" /> <PackageReference Include="Serilog.Sinks.Console" Version="6.0.0" /> <PackageReference Include="Serilog.Sinks.File" Version="7.0.0" /> </ItemGroup> <ItemGroup> <ProjectReference Include="..\BookingService.Booking.Api.Contracts\BookingService.Booking.Api.Contracts.csproj" /> <ProjectReference Include="..\BookingService.Booking.AppServices\BookingService.Booking.AppServices.csproj" /> <ProjectReference Include="..\BookingService.Booking.Domain\BookingService.Booking.Domain.csproj" /> <ProjectReference Include="..\BookingService.Booking.Persistence\BookingService.Booking.Persistence.csproj" /> </ItemGroup> <ItemGroup> <PackageReference Include="Swashbuckle.AspNetCore" Version="6.*" /> </ItemGroup> <ItemGroup> <Content Update="appsettings.Development.json"> <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory> <ExcludeFromSingleFile>true</ExcludeFromSingleFile> <CopyToPublishDirectory>PreserveNewest</CopyToPublishDirectory> </Content> <Content Update="appsettings.json"> <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory> <ExcludeFromSingleFile>true</ExcludeFromSingleFile> <CopyToPublishDirectory>PreserveNewest</CopyToPublishDirectory> </Content> <Content Update="appsettings.Production.json"> <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory> <ExcludeFromSingleFile>true</ExcludeFromSingleFile> <CopyToPublishDirectory>PreserveNewest</CopyToPublishDirectory> </Content> </ItemGroup> </Project>
```

# BookingService.Booking.Host\Controllers\BookingsController.cs

```cs
using BookingService.Booking.Api.Contracts.Bookings; using BookingService.Booking.Api.Contracts.Bookings.Requests; using BookingService.Booking.AppServices.Bookings; using BookingService.Booking.AppServices; using BookingService.Booking.Host.Mapping; using Microsoft.AspNetCore.Mvc; namespace BookingService.Booking.Host.Controllers; [ApiController] public class BookingsController : ControllerBase { private readonly IBookingsQueries _bookingsQueries; private readonly IBookingsService _bookingsService; public BookingsController(IBookingsService bookingsService, IBookingsQueries bookingsQueries) { _bookingsService = bookingsService; _bookingsQueries = bookingsQueries; } [HttpPost] [Route(WebRoutes.Create)] public async Task<long> CreateBooking([FromBody] CreateBookingRequest createBookingRequest, CancellationToken cancellationToken) { return await _bookingsService.Create(createBookingRequest.ToQuery(), cancellationToken); } [HttpGet] [Route(WebRoutes.GetByFilter)] public async Task<BookingData[]> GetBookingsByFilter([FromQuery] GetBookingsByFilterRequest getBookingsByFilter, CancellationToken cancellationToken) { return await _bookingsQueries.GetByFilter(getBookingsByFilter.ToQuery(), cancellationToken); } [HttpPost] [Route(WebRoutes.Cancel)] public async Task CancelBooking([FromBody] long id, CancellationToken cancellationToken) { await _bookingsService.Cancel(id, cancellationToken); } [HttpGet] [Route(WebRoutes.GetById)] public async Task<BookingData> GetBookingById([FromRoute] long id, CancellationToken cancellationToken) { return await _bookingsService.GetById(id, cancellationToken); } [HttpGet] [Route(WebRoutes.GetStatusById)] public async Task<string> GetBookingStatusById([FromRoute] long id, CancellationToken cancellationToken) { return await _bookingsQueries.GetStatusById(id, cancellationToken); } }
```

# BookingService.Booking.Host\Dockerfile

```
# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки. # Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки) FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base USER $APP_UID WORKDIR /app EXPOSE 8080 EXPOSE 8081 # Этот этап используется для сборки проекта службы FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build ARG BUILD_CONFIGURATION=Release WORKDIR /src COPY ["src/BookingService.Booking.Host/BookingService.Booking.Host.csproj", "BookingService.Booking.Host/"] COPY ["src/BookingService.Booking.Api.Contracts/BookingService.Booking.Api.Contracts.csproj", "BookingService.Booking.Api.Contracts/"] COPY ["src/BookingService.Booking.AppServices/BookingService.Booking.AppServices.csproj", "BookingService.Booking.AppServices/"] RUN dotnet restore "./BookingService.Booking.Host/BookingService.Booking.Host.csproj" COPY . . WORKDIR "/src/BookingService.Booking.Host" RUN dotnet build "src/BookingService.Booking.Host/BookingService.Booking.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build # Этот этап используется для публикации проекта службы, который будет скопирован на последний этап FROM build AS publish ARG BUILD_CONFIGURATION=Release RUN dotnet publish "./BookingService.Booking.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false # Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется) FROM base AS final WORKDIR /app COPY --from=publish /app/publish . ENTRYPOINT ["dotnet", "BookingService.Booking.Host.dll"]
```

# BookingService.Booking.Host\HostBuilderFactory.cs

```cs
using Serilog; namespace BookingService.Booking.Host; public static class HostBuilderFactory { public static IHost BuildHost(string[] args) { return Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args) .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); }) .UseSerilog() // Подключение Serilog .Build(); } }
```

# BookingService.Booking.Host\Mapping\BookingMappings.cs

```cs
using BookingService.Booking.Api.Contracts.Bookings.Requests; using BookingService.Booking.AppServices.Queries; namespace BookingService.Booking.Host.Mapping; public static class BookingMappings { public static CreateBookingQuery ToQuery(this CreateBookingRequest request) { return new CreateBookingQuery { IdUser = request.IdUser, IdBooking = request.IdBooking, StartBooking = request.StartBooking, EndBooking = request.EndBooking }; } public static GetBookingsByFilterQuery ToQuery(this GetBookingsByFilterRequest request) { return new GetBookingsByFilterQuery { Id = request.Id, IdUser = request.IdUser, IdBooking = request.IdBooking, Status = request.Status, CreationBooking = request.CreationBooking, StartBooking = request.StartBooking, EndBooking = request.EndBooking }; } }
```

# BookingService.Booking.Host\Program.cs

```cs
//var builder = WebApplication.CreateBuilder(args); //var app = builder.Build(); //app.MapGet("/", () => "Hello World!"); //app.Run(); using BookingService.Booking.Host; var host = HostBuilderFactory.BuildHost(args); host.Run();
```

# BookingService.Booking.Host\Properties\launchSettings.json

```json
{ "profiles": { "BookingService.Booking.Host": { "commandName": "Project", "launchBrowser": true, "environmentVariables": { "ASPNETCORE_ENVIRONMENT": "Development" }, "applicationUrl": "https://localhost:63915;http://localhost:63916" }, "Container (Dockerfile)": { "commandName": "Docker", "launchBrowser": true, "launchUrl": "{Scheme}://{ServiceHost}:{ServicePort}", "environmentVariables": { "ASPNETCORE_HTTPS_PORTS": "8081", "ASPNETCORE_HTTP_PORTS": "8080" }, "publishAllPorts": true, "useSSL": true } } }
```

# BookingService.Booking.Host\ServiceCollectionExtensions.cs

```cs
using BookingService.Booking.AppServices.Bookings.Jobs; using BookingService.Booking.AppServices; namespace BookingService.Booking.Host; public static class ServiceCollectionExtensions { public static void AddAppServices(this IServiceCollection services) { services.AddScoped<IBookingsBackgroundServiceHandler, BookingsBackgroundServiceHandler>(); services.AddHostedService<BookingsBackgroundService>(); } }
```

# BookingService.Booking.Host\Startup.cs

```cs
using BookingService.Booking.AppServices.Bookings; using BookingService.Booking.AppServices.Exceptions; using BookingService.Booking.Domain.Exceptions; using BookingService.Booking.Persistence; using Hellang.Middleware.ProblemDetails; using Microsoft.AspNetCore.Mvc; using Microsoft.OpenApi.Models; namespace BookingService.Booking.Host; public class Startup { private readonly IConfiguration _configuration; public Startup(IConfiguration configuration) { _configuration = configuration; } // Настройка сервисов public void ConfigureServices(IServiceCollection services) { // Регистрация сервисов в DI-контейнере services.AddControllers(); // Добавление Swagger services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Booking Service API", Version = "v1", Description = "API для сервиса бронирования" }); }); services.AddAppServices(_configuration); services.AddPersistence(_configuration.GetConnectionString("BookingsContext")); services.AddAppServices(); services.AddProblemDetails(options => { // Если окружение Development, включаем подробное описание ошибки в ответ. options.IncludeExceptionDetails = (context, _) => { var env = context.RequestServices.GetRequiredService<IWebHostEnvironment>(); return env.IsDevelopment(); }; options.Map<DomainException>(ex => new ProblemDetails { Status = 402, Type = $"https://httpstatuses.com/{402}", Title = ex.Message, Detail = ex.StackTrace }); options.Map<ValidationException>(ex => new ProblemDetails { Status = 400, Type = $"https://httpstatuses.com/{400}", Title = ex.Message }); }); } // Настройка middleware public void Configure(IApplicationBuilder app, IWebHostEnvironment env) { if (env.IsDevelopment()) { app.UseDeveloperExceptionPage(); app.UseSwagger(); app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Booking Service API V1"); }); } app.UseRouting(); app.UseEndpoints(endpoints => { endpoints.MapControllers(); }); app.UseProblemDetails(); } }
```

# BookingService.Booking.Persistence\appsettings.json

```json
{ "ConnectionStrings": { "BookingsContext": "server=localhost;port=5433;database=booking_service_bookings;uid=bookings_admin;pwd=admin_bookings" } }
```

# BookingService.Booking.Persistence\BookingQueries.cs

```cs
using BookingService.Booking.AppServices; using BookingService.Booking.AppServices.Bookings; using BookingService.Booking.AppServices.Queries; using BookingService.Booking.Domain.Bookings; using Microsoft.EntityFrameworkCore; namespace BookingService.Booking.Persistence; internal class BookingQueries : IBookingsQueries { private readonly BookingsContext _context; public BookingQueries(BookingsContext context) { _context = context; } public async Task<BookingData[]> GetByFilter(GetBookingsByFilterQuery getBookingsByFilter, CancellationToken cancellationToken) { var bookingsQuery = _context.Bookings.AsQueryable(); if (getBookingsByFilter.Id.HasValue) bookingsQuery = bookingsQuery .Where(x => x.Id == getBookingsByFilter.Id.Value); if (getBookingsByFilter.Status.HasValue) bookingsQuery = bookingsQuery .Where(x => x.Status == getBookingsByFilter.Status.Value); if (getBookingsByFilter.IdUser.HasValue) bookingsQuery = bookingsQuery .Where(x => x.IdUser == getBookingsByFilter.IdUser.Value); if (getBookingsByFilter.IdBooking.HasValue) bookingsQuery = bookingsQuery .Where(x => x.IdBooking == getBookingsByFilter.IdBooking.Value); if (getBookingsByFilter.StartBooking.HasValue) bookingsQuery = bookingsQuery .Where(x => x.StartBooking == getBookingsByFilter.StartBooking.Value); if (getBookingsByFilter.EndBooking.HasValue) bookingsQuery = bookingsQuery .Where(x => x.EndBooking == getBookingsByFilter.EndBooking.Value); if (getBookingsByFilter.CreationBooking.HasValue) bookingsQuery = bookingsQuery .Where(x => x.CreationBooking == getBookingsByFilter.CreationBooking.Value); return await bookingsQuery.Select(x => x.ToBookingData()).ToArrayAsync(cancellationToken); } public async Task<string> GetStatusById(long id, CancellationToken cancellationToken) { var booking = await _context.FindAsync<BookingAggregate>(id, cancellationToken); return booking.Status.ToString(); } }
```

# BookingService.Booking.Persistence\BookingsBackgroundQueries.cs

```cs
using BookingService.Booking.Domain.Bookings; using BookingService.Booking.Domain.Contracts.Bookings; namespace BookingService.Booking.Persistence { public class BookingsBackgroundQueries : IBookingsBackgroundQueries { private BookingsContext _bookingsContext; public BookingsBackgroundQueries(BookingsContext bookingsContext) { _bookingsContext = bookingsContext; } public IReadOnlyCollection<BookingAggregate> GetConfirmationAwaitingBookings(int countBookings = 10) { return _bookingsContext.Bookings .Where(x => x.Status == BookingStatus.AwaitsConfirmation) .OrderBy(x => x.Id) .Take(countBookings) .ToList() .AsReadOnly(); } } }
```

# BookingService.Booking.Persistence\BookingsContext.cs

```cs
using BookingService.Booking.Domain.Bookings; using BookingService.Booking.Persistence.Configurations; using Microsoft.EntityFrameworkCore; namespace BookingService.Booking.Persistence; public class BookingsContext : DbContext { public BookingsContext(DbContextOptions<BookingsContext> options) : base(options) { } public DbSet<BookingAggregate> Bookings { get; set; } protected override void OnModelCreating(ModelBuilder modelBuilder) { modelBuilder.ApplyConfiguration(new BookingAggregateConfiguration()); base.OnModelCreating(modelBuilder); } }
```

# BookingService.Booking.Persistence\BookingService.Booking.Persistence.csproj

```csproj
<Project Sdk="Microsoft.NET.Sdk"> <PropertyGroup> <TargetFramework>net8.0</TargetFramework> <ImplicitUsings>enable</ImplicitUsings> <Nullable>enable</Nullable> </PropertyGroup> <ItemGroup> <None Update="appsettings.json"> <CopyToOutputDirectory>Always</CopyToOutputDirectory> </None> </ItemGroup> <ItemGroup> <PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.8" /> <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.8"> <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets> <PrivateAssets>all</PrivateAssets> </PackageReference> <PackageReference Include="Microsoft.EntityFrameworkCore.Relational" Version="8.0.8" /> <PackageReference Include="Microsoft.Extensions.Configuration" Version="8.0.0" /> <PackageReference Include="Microsoft.Extensions.Configuration.Binder" Version="8.0.0" /> <PackageReference Include="Microsoft.Extensions.Configuration.Json" Version="8.0.0" /> <PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="8.0.4" /> </ItemGroup> <ItemGroup> <ProjectReference Include="..\BookingService.Booking.AppServices\BookingService.Booking.AppServices.csproj" /> <ProjectReference Include="..\BookingService.Booking.Domain\BookingService.Booking.Domain.csproj" /> </ItemGroup> </Project>
```

# BookingService.Booking.Persistence\BookingsRepository.cs

```cs
using BookingService.Booking.AppServices; using BookingService.Booking.Domain.Bookings; using Microsoft.EntityFrameworkCore; namespace BookingService.Booking.Persistence; public class BookingsRepository : IBookingsRepository { private readonly DbSet<BookingAggregate> _dbset; public BookingsRepository(BookingsContext context) { _dbset = context.Bookings; } public async Task Create(BookingAggregate aggregate, CancellationToken cancellationToken) { await _dbset.AddAsync(aggregate, cancellationToken); } public async ValueTask<BookingAggregate?> GetById(long id, CancellationToken token = default) { return await _dbset.FindAsync(id, token); } public async Task Update(BookingAggregate aggregate, CancellationToken cancellationToken) { _dbset.Attach(aggregate); _dbset.Entry(aggregate).State = EntityState.Modified; } }
```

# BookingService.Booking.Persistence\Configurations\BookingAggregateConfiguration.cs

```cs
using BookingService.Booking.Domain.Bookings; using Microsoft.EntityFrameworkCore; using Microsoft.EntityFrameworkCore.Metadata.Builders; namespace BookingService.Booking.Persistence.Configurations; public class BookingAggregateConfiguration : IEntityTypeConfiguration<BookingAggregate> { public void Configure(EntityTypeBuilder<BookingAggregate> builder) { builder.ToTable("bookings"); builder.HasKey(x => x.Id) .HasName("pk_bookings"); builder.Property(x => x.Id) .HasColumnName("id") .HasColumnType("bigint"); builder.Property(x => x.CatalogRequestId) .HasColumnName("catalog_request_id") .HasColumnType("uuid"); builder.Property(x => x.Status) .HasColumnName("status") .HasColumnType("int"); builder.Property(x => x.IdUser) .HasColumnName("user_id") .HasColumnType("bigint"); builder.Property(x => x.IdBooking) .HasColumnName("resource_id") .HasColumnType("bigint"); builder.Property(x => x.StartBooking) .HasColumnName("start_date") .HasColumnType("date"); builder.Property(x => x.EndBooking) .HasColumnName("end_date") .HasColumnType("date"); builder.Property(x => x.CreationBooking) .HasColumnName("created_at_date_time") .HasColumnType("timestamptz"); } }
```

# BookingService.Booking.Persistence\DesignTimeDbContextFactory.cs

```cs
using Microsoft.EntityFrameworkCore; using Microsoft.EntityFrameworkCore.Design; using Microsoft.Extensions.Configuration; namespace BookingService.Booking.Persistence; public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BookingsContext> { public BookingsContext CreateDbContext(string[] args) { var optionsBuilder = new DbContextOptionsBuilder<BookingsContext>(); var configuration = new ConfigurationBuilder() .SetBasePath(AppDomain.CurrentDomain.BaseDirectory) .AddJsonFile("appsettings.json", false, true) .Build(); var connectionString = configuration.GetConnectionString(nameof(BookingsContext)); if (string.IsNullOrWhiteSpace(connectionString)) throw new InvalidOperationException($"ConnectionString for `{nameof(BookingsContext)}` not found"); optionsBuilder.UseNpgsql(connectionString); return new BookingsContext(optionsBuilder.Options); } }
```

# BookingService.Booking.Persistence\Migrations\20250611221234_InitialMigration.cs

```cs
using Microsoft.EntityFrameworkCore.Migrations; using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata; #nullable disable namespace BookingService.Booking.Persistence.Migrations { /// <inheritdoc /> public partial class InitialMigration : Migration { /// <inheritdoc /> protected override void Up(MigrationBuilder migrationBuilder) { migrationBuilder.CreateTable( name: "bookings", columns: table => new { id = table.Column<long>(type: "bigint", nullable: false) .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn), status = table.Column<int>(type: "int", nullable: false), user_id = table.Column<long>(type: "bigint", nullable: false), resource_id = table.Column<long>(type: "bigint", nullable: false), start_date = table.Column<DateOnly>(type: "date", nullable: false), end_date = table.Column<DateOnly>(type: "date", nullable: false), created_at_date_time = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false) }, constraints: table => { table.PrimaryKey("pk_bookings", x => x.id); }); } /// <inheritdoc /> protected override void Down(MigrationBuilder migrationBuilder) { migrationBuilder.DropTable( name: "bookings"); } } }
```

# BookingService.Booking.Persistence\Migrations\20250611221234_InitialMigration.Designer.cs

```cs
// <auto-generated /> using System; using BookingService.Booking.Persistence; using Microsoft.EntityFrameworkCore; using Microsoft.EntityFrameworkCore.Infrastructure; using Microsoft.EntityFrameworkCore.Migrations; using Microsoft.EntityFrameworkCore.Storage.ValueConversion; using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata; #nullable disable namespace BookingService.Booking.Persistence.Migrations { [DbContext(typeof(BookingsContext))] [Migration("20250611221234_InitialMigration")] partial class InitialMigration { /// <inheritdoc /> protected override void BuildTargetModel(ModelBuilder modelBuilder) { #pragma warning disable 612, 618 modelBuilder .HasAnnotation("ProductVersion", "8.0.8") .HasAnnotation("Relational:MaxIdentifierLength", 63); NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder); modelBuilder.Entity("BookingService.Booking.Domain.Bookings.BookingAggregate", b => { b.Property<long>("Id") .ValueGeneratedOnAdd() .HasColumnType("bigint") .HasColumnName("id"); NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id")); b.Property<DateTimeOffset>("CreationBooking") .HasColumnType("timestamptz") .HasColumnName("created_at_date_time"); b.Property<DateOnly>("EndBooking") .HasColumnType("date") .HasColumnName("end_date"); b.Property<long>("IdBooking") .HasColumnType("bigint") .HasColumnName("resource_id"); b.Property<long>("IdUser") .HasColumnType("bigint") .HasColumnName("user_id"); b.Property<DateOnly>("StartBooking") .HasColumnType("date") .HasColumnName("start_date"); b.Property<int>("Status") .HasColumnType("int") .HasColumnName("status"); b.HasKey("Id") .HasName("pk_bookings"); b.ToTable("bookings", (string)null); }); #pragma warning restore 612, 618 } } }
```

# BookingService.Booking.Persistence\Migrations\20250621174526_AddCatalogRequestId.cs

```cs
using System; using Microsoft.EntityFrameworkCore.Migrations; #nullable disable namespace BookingService.Booking.Persistence.Migrations { /// <inheritdoc /> public partial class AddCatalogRequestId : Migration { /// <inheritdoc /> protected override void Up(MigrationBuilder migrationBuilder) { migrationBuilder.AddColumn<Guid>( name: "catalog_request_id", table: "bookings", type: "uuid", nullable: true); } /// <inheritdoc /> protected override void Down(MigrationBuilder migrationBuilder) { migrationBuilder.DropColumn( name: "catalog_request_id", table: "bookings"); } } }
```

# BookingService.Booking.Persistence\Migrations\20250621174526_AddCatalogRequestId.Designer.cs

```cs
// <auto-generated /> using System; using BookingService.Booking.Persistence; using Microsoft.EntityFrameworkCore; using Microsoft.EntityFrameworkCore.Infrastructure; using Microsoft.EntityFrameworkCore.Migrations; using Microsoft.EntityFrameworkCore.Storage.ValueConversion; using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata; #nullable disable namespace BookingService.Booking.Persistence.Migrations { [DbContext(typeof(BookingsContext))] [Migration("20250621174526_AddCatalogRequestId")] partial class AddCatalogRequestId { /// <inheritdoc /> protected override void BuildTargetModel(ModelBuilder modelBuilder) { #pragma warning disable 612, 618 modelBuilder .HasAnnotation("ProductVersion", "8.0.8") .HasAnnotation("Relational:MaxIdentifierLength", 63); NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder); modelBuilder.Entity("BookingService.Booking.Domain.Bookings.BookingAggregate", b => { b.Property<long>("Id") .ValueGeneratedOnAdd() .HasColumnType("bigint") .HasColumnName("id"); NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id")); b.Property<Guid?>("CatalogRequestId") .HasColumnType("uuid") .HasColumnName("catalog_request_id"); b.Property<DateTimeOffset>("CreationBooking") .HasColumnType("timestamptz") .HasColumnName("created_at_date_time"); b.Property<DateOnly>("EndBooking") .HasColumnType("date") .HasColumnName("end_date"); b.Property<long>("IdBooking") .HasColumnType("bigint") .HasColumnName("resource_id"); b.Property<long>("IdUser") .HasColumnType("bigint") .HasColumnName("user_id"); b.Property<DateOnly>("StartBooking") .HasColumnType("date") .HasColumnName("start_date"); b.Property<int>("Status") .HasColumnType("int") .HasColumnName("status"); b.HasKey("Id") .HasName("pk_bookings"); b.ToTable("bookings", (string)null); }); #pragma warning restore 612, 618 } } }
```

# BookingService.Booking.Persistence\Migrations\BookingsContextModelSnapshot.cs

```cs
// <auto-generated /> using System; using BookingService.Booking.Persistence; using Microsoft.EntityFrameworkCore; using Microsoft.EntityFrameworkCore.Infrastructure; using Microsoft.EntityFrameworkCore.Storage.ValueConversion; using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata; #nullable disable namespace BookingService.Booking.Persistence.Migrations { [DbContext(typeof(BookingsContext))] partial class BookingsContextModelSnapshot : ModelSnapshot { protected override void BuildModel(ModelBuilder modelBuilder) { #pragma warning disable 612, 618 modelBuilder .HasAnnotation("ProductVersion", "8.0.8") .HasAnnotation("Relational:MaxIdentifierLength", 63); NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder); modelBuilder.Entity("BookingService.Booking.Domain.Bookings.BookingAggregate", b => { b.Property<long>("Id") .ValueGeneratedOnAdd() .HasColumnType("bigint") .HasColumnName("id"); NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id")); b.Property<Guid?>("CatalogRequestId") .HasColumnType("uuid") .HasColumnName("catalog_request_id"); b.Property<DateTimeOffset>("CreationBooking") .HasColumnType("timestamptz") .HasColumnName("created_at_date_time"); b.Property<DateOnly>("EndBooking") .HasColumnType("date") .HasColumnName("end_date"); b.Property<long>("IdBooking") .HasColumnType("bigint") .HasColumnName("resource_id"); b.Property<long>("IdUser") .HasColumnType("bigint") .HasColumnName("user_id"); b.Property<DateOnly>("StartBooking") .HasColumnType("date") .HasColumnName("start_date"); b.Property<int>("Status") .HasColumnType("int") .HasColumnName("status"); b.HasKey("Id") .HasName("pk_bookings"); b.ToTable("bookings", (string)null); }); #pragma warning restore 612, 618 } } }
```

# BookingService.Booking.Persistence\ServiceCollectionExtensions.cs

```cs
using BookingService.Booking.Domain; using BookingService.Booking.Domain.Bookings; using Microsoft.EntityFrameworkCore; using Microsoft.Extensions.DependencyInjection; using Microsoft.Extensions.Logging; using BookingService.Booking.AppServices; namespace BookingService.Booking.Persistence; public static class ServiceCollectionExtensions { public static IServiceCollection AddPersistence(this IServiceCollection services, string connectionString) { if (services == null) throw new ArgumentNullException(nameof(services)); if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentNullException(nameof(connectionString)); services.AddScoped<IBookingsRepository, BookingsRepository>(); services.AddScoped<IUnitOfWork, UnitOfWork>(); services.AddScoped<IBookingsQueries, BookingQueries>(); services.AddScoped<IBookingsBackgroundQueries, BookingsBackgroundQueries>(); services.AddDbContext<BookingsContext>((ctx, context) => { context.UseNpgsql(connectionString) .UseLoggerFactory(ctx.GetRequiredService<ILoggerFactory>()); } ); return services; } }
```

# BookingService.Booking.Persistence\UnitOfWork.cs

```cs
using BookingService.Booking.AppServices; using BookingService.Booking.Domain; namespace BookingService.Booking.Persistence; public class UnitOfWork : IUnitOfWork { private readonly BookingsContext _dbContext; public UnitOfWork(BookingsContext dbContext, IBookingsRepository bookingsRepository) { _dbContext = dbContext; BookingsRepository = bookingsRepository; } public IBookingsRepository BookingsRepository { get; } public async Task CommitAsync(CancellationToken cancellationToken = default) { await _dbContext.SaveChangesAsync(cancellationToken); } }
```

