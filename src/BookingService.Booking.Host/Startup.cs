using BookingService.Booking.AppServices.Bookings;
using BookingService.Booking.AppServices.Exceptions;
using BookingService.Booking.Domain.Exceptions;
using BookingService.Booking.Persistence;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
namespace BookingService.Booking.Host
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Настройка сервисов
        public void ConfigureServices(IServiceCollection services)
        {
            // Регистрация сервисов в DI-контейнере
            services.AddControllers();

            // Добавление Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Booking Service API", Version = "v1", Description = "API для сервиса бронирования" });

            });

            services.AddAppServices();
            services.AddPersistence(_configuration.GetConnectionString("BookingsContext"));
          //  services.AddAppServices();

            services.AddProblemDetails(options =>
            {
                // Если окружение Development, включаем подробное описание ошибки в ответ.
                options.IncludeExceptionDetails = (context, _) =>
                {
                    var env = context.RequestServices.GetRequiredService<IWebHostEnvironment>();
                    return env.IsDevelopment();
                };
                options.Map<DomainException>(ex => new ProblemDetails
                {
                    Status = 402,
                    Type = $"https://httpstatuses.com/{402}",
                    Title = ex.Message,
                    Detail = ex.StackTrace
                });
                options.Map<ValidationException>(ex => new ProblemDetails
                {
                    Status = 400,
                    Type = $"https://httpstatuses.com/{400}",
                    Title = ex.Message,
                });
            });
        }
        // Настройка middleware
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Booking Service API V1");
                });
            }

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseProblemDetails();

        }
    }
}
