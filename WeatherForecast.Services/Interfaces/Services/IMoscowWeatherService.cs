using WeatherForecast.Domain.Models;

namespace WeatherForecast.Services.Interfaces.Services
{
    public interface IMoscowWeatherService
    {
        Task<MoscowWeather> GetWeather(DateTime date, CancellationToken cancellationToken);
        Task<MoscowWeather> SaveWeather(DateTime date, CancellationToken cancellationToken);
    }
}
