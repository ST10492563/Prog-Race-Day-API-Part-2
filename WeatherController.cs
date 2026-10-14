using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayApi.Data;
using RaceDayApi.Models;
using RaceDayApi.Security;

namespace RaceDayApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public WeatherController(RaceDayDbContext context)
        {
            _context = context;
        }

        [HttpGet("{eventId}")]
        public async Task<IActionResult> GetWeather(int eventId)
        {
            var weather = await _context.EventWeather
                .Where(w => w.EventID == eventId)
                .OrderByDescending(w => w.ForecastDate)
                .ToListAsync();

            return Ok(weather);
        }

        [HttpPost]
        public async Task<IActionResult> AddWeather(
            EventWeather weather)
        {
            if (!SessionHelper.IsOrganiser(HttpContext))
                return Unauthorized(
                    "Only organisers can add weather information.");

            weather.RecordedAt = DateTime.Now;

            _context.EventWeather.Add(weather);

            await _context.SaveChangesAsync();

            return Ok(weather);
        }
    }
}
