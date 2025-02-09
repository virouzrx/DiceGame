using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace DiceGame.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("RedisTest")]
        public async Task<IActionResult> RedisTest()
        {
            //ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("redis-16919.c251.east-us-mz.azure.redns.redis-cloud.com:16919,password=HtetyyIXMWi6VM1yEE14h6TIqqwz3PHP");
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:6379");
            var db = redis.GetDatabase();

            await db.StringSetAsync("testKey", "12345678");
            var value = await db.StringGetAsync("testKey");
            return Ok(value.ToString());
        }
    }
}