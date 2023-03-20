using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using WeatherForecast.Domain.Common;
using WeatherForecast.Domain.Models;
using WeatherForecast.Infrastructure.EntityFramework;
using WeatherForecast.Services.Interfaces.Services;

namespace WeatherForecast.Services.Services
{
    public sealed class AuthService : IAuthService
    {
        private readonly DataContext _context;

        public AuthService(DataContext context)
        {
            _context = context;
        }

        public async Task<AuthResponse> Register(User user, string password, CancellationToken cancellationToken)
        {
            var response = new AuthResponse();
            if (await UserExists(user.Username, cancellationToken))
            {
                response.IsSuccess = false;
                response.Message = "User already exists.";
                return response;
            }
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] securityKey);

            user.PasswordHash = passwordHash;
            user.SecurityKey = securityKey;

            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            response.IsSuccess = true;
            return response;
        }

        public async Task<bool> CanLogIn(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username.ToLower().Equals(username.ToLower()));
            if (user == null)
            {
                return false;
            }
            else if (!VerifyPasswordHash(password, user.PasswordHash, user.SecurityKey))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] securityKey)
        {
            using (var hmac = new HMACSHA512(securityKey))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        private async Task<bool> UserExists(string username, CancellationToken cancellationToken)
        {
            if (await _context.Users.AnyAsync(x => x.Username.ToLower() == username.ToLower(), cancellationToken))
            {
                return true;
            }
            return false;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] securityKey)
        {
            using (var hmac = new HMACSHA512())
            {
                securityKey = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
