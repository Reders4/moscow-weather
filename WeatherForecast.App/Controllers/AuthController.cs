using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeatherForecast.Domain.Common;
using WeatherForecast.Domain.Models;
using WeatherForecast.Services.Interfaces.Services;

namespace WeatherForecast.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authRepository;
        public AuthController(IAuthService authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AuthResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] UserRegister user, CancellationToken cancellationToken)
        {
            var response = await _authRepository.Register(new User { Username = user.Login }, user.Password, cancellationToken);
            if (response.IsSuccess)
            {
                return Created(new Uri($"api/Auth/{response}", UriKind.Relative), response);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
