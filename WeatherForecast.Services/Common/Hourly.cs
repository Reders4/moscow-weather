namespace WeatherForecast.Services.Common
{
    internal class Hourly
    {
        public List<DateTime> time { get; set; }
        public List<double> temperature_2m { get; set; }
    }
}
