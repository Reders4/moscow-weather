using Newtonsoft.Json;
using WeatherForecast.Domain.Models;
using WeatherForecast.Infrastructure.EntityFramework;
using WeatherForecast.Services.Common;
using WeatherForecast.Services.Interfaces.Services;

namespace WeatherForecast.Services.Services
{
    public sealed class MoscowWeatherService : IMoscowWeatherService
    {
        private readonly DataContext _context;
        private readonly HttpClient _client;
        public MoscowWeatherService(DataContext context, HttpClient client)
        {
            _context = context;
            _client = client;
        }
        public async Task<MoscowWeather> GetWeather(DateTime date, CancellationToken cancellationToken)
        {
            var response = await _client.GetAsync(GetUriByDate(ConvertDateFormat(date)), cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var convertedModel = JsonConvert.DeserializeObject<OpenMeteoJsonModel>(await response.Content.ReadAsStringAsync(cancellationToken));
                return new MoscowWeather
                {
                    Date = date,
                    Temperature = convertedModel.hourly.temperature_2m.First()
                };
            }
            else
            {
                throw new ArgumentException("На указанную дату в Open-Meteo нет данных");
            }

        }

        public async Task<MoscowWeather> SaveWeather(DateTime date, CancellationToken cancellationToken)
        {
            var weather = await GetWeather(date, cancellationToken);
            _context.MoscowWeathers.Add(weather);
            await _context.SaveChangesAsync(cancellationToken);
            return weather;
        }

        private string GetUriByDate(string formatedDate)
        {
            return $"https://api.open-meteo.com/v1/forecast?latitude=55.75&longitude=37.62&hourly=temperature_2m&start_date={formatedDate}&end_date={formatedDate}";
        }

        private string ConvertDateFormat(DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }
    }
}
