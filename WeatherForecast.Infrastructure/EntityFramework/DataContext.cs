using Microsoft.EntityFrameworkCore;
using WeatherForecast.Domain.Models;
using WeatherForecast.Infrastructure.EntityFramework.EntityModels;

namespace WeatherForecast.Infrastructure.EntityFramework
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public virtual DbSet<MoscowWeather> MoscowWeathers { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MoscowWeatherEntityModel).Assembly);
        }
    }
}
