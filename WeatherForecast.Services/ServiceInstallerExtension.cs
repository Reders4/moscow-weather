using Microsoft.Extensions.DependencyInjection;
using WeatherForecast.Services.Interfaces.Services;
using WeatherForecast.Services.Services;

namespace WeatherForecast.Services
{
    public static class ServiceInstallerExtension
    {
        public static IServiceCollection AddWeatherForecastServices(this IServiceCollection services)
        {
            services.AddScoped<IMoscowWeatherService, MoscowWeatherService>();
            services.AddScoped<IAuthService, AuthService>();
            return services;
        }
    }
}
