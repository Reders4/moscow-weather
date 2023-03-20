using WeatherForecast.Domain.Common;
using WeatherForecast.Domain.Models;

namespace WeatherForecast.Services.Interfaces.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> Register(User user, string password, CancellationToken cancellationToken);
        Task<bool> CanLogIn(string username, string password);
    }
}
