using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeatherForecast.Domain.Models;

namespace WeatherForecast.Infrastructure.EntityFramework.EntityModels
{
    internal class MoscowWeatherEntityModel : IEntityTypeConfiguration<MoscowWeather>
    {
        public void Configure(EntityTypeBuilder<MoscowWeather> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Date).HasConversion(
                src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc),
                dst => dst.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst, DateTimeKind.Utc)
                ).IsRequired();
            builder.Property(x => x.Temperature).IsRequired();
        }
    }
}
