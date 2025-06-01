using Microsoft.OpenApi.Models;
using BookingService.Booking.AppServices.Bookings;
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
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Booking Service API", Version = "v1", Description = "API для сервиса бронирования" });

            });
            services.AddAppServices();
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
            // Другие middleware...
        }
    }
}
