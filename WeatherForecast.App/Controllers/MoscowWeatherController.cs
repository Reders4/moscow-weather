using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WeatherForecast.Domain.Models;
using WeatherForecast.Services.Interfaces.Services;

namespace WeatherForecast.App.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MoscowWeatherController : ControllerBase
    {
        private readonly IMoscowWeatherService _moscowWeatherService;
        public MoscowWeatherController(IMoscowWeatherService moscowWeatherService)
        {
            _moscowWeatherService = moscowWeatherService;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MoscowWeather>> MoscowWeather([Required] DateTime date, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _moscowWeatherService.GetWeather(date, cancellationToken);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {

                return NotFound(ex.Message);

            }
            
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(MoscowWeather))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MoscowWeather>> SaveMoscowWeather([Required] DateTime date, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _moscowWeatherService.SaveWeather(date, cancellationToken);
                return Created(new Uri($"api/MoscowWeather/{response}", UriKind.Relative), response);
            }
            catch (ArgumentException ex)
            {

                return BadRequest(ex.Message);
            }
            
        }
    }
}
