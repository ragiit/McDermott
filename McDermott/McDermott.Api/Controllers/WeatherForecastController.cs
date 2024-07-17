using MediatR;
using Microsoft.AspNetCore.Mvc;
using static McDermott.Application.Features.Commands.Config.UserCommand;

namespace McDermott.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController(ILogger<WeatherForecastController> logger, IMediator mediator) : ControllerBase
    {
        private static readonly string[] _summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        private readonly ILogger<WeatherForecastController> _logger = logger;

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = _summaries[Random.Shared.Next(_summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("a")]
        public async Task<IActionResult> A()
        {
            var aa = await mediator.Send(new GetUserQuery());

            return Ok(aa);
        }
    }
}
