using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WeatherApp.Services;

namespace WeatherApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _service;
        public WeatherController(IWeatherService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string city)
        {
            var json = await _service.GetWeatherAsync(city);
            if (json is null) return NotFound();

            var doc = JsonDocument.Parse(json);
            var area = doc.RootElement
                .GetProperty("nearest_area")[0]
                .GetProperty("areaName")[0]
                .GetProperty("value").GetString();

            var cond = doc.RootElement.GetProperty("current_condition")[0];
            var desc = cond.GetProperty("weatherDesc")[0].GetProperty("value").GetString();
            var tempStr = cond.GetProperty("temp_C").GetString();
            double.TryParse(tempStr, out var temp);

            return Ok(new 
            {
                City = area ?? city, 
                Description = desc, 
                Temperature = temp 
            });
        }
    }
}
