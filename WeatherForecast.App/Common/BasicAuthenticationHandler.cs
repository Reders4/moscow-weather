﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using WeatherForecast.Services.Interfaces.Services;

namespace WeatherForecast.App.Common
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IAuthService _authService;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,UrlEncoder encoder, ISystemClock clock,
            IAuthService authService)
            : base(options, logger, encoder, clock)
        {
            _authService = authService;
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.Headers["WWW-Authenticate"] = "Basic";
            return base.HandleChallengeAsync(properties);
        }
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string username;
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter)).Split(':');
                username = credentials.FirstOrDefault();
                var password = credentials.LastOrDefault();

                if (!await _authService.CanLogIn(username, password))
                {
                    throw new ArgumentException("Invalid username or password");
                }
            }
            catch (Exception ex)
            {

                return AuthenticateResult.Fail(ex.Message);
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username)
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}