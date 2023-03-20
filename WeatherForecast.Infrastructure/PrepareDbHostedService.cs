using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WeatherForecast.Infrastructure.EntityFramework;

namespace WeatherForecast.Infrastructure
{
    public class PrepareDbHostedService : BackgroundService
    {
        private readonly DataContext _conext;
        private readonly ILogger<PrepareDbHostedService> _logger;
        public PrepareDbHostedService(DataContext conext, ILogger<PrepareDbHostedService> logger)
        {
            _conext = conext;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await _conext.Database.MigrateAsync(stoppingToken);
                await StopAsync(stoppingToken);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "PrepareDb failed");
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("PrepareDb Hosted Service is stopping");
            await base.StopAsync(cancellationToken);
        }
    }
}
