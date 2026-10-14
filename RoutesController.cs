using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayApi.Data;
using RaceDayRoute = RaceDayApi.Models.Route;
using RaceDayApi.Security;

namespace RaceDayApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoutesController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public RoutesController(RaceDayDbContext context)
        {
            _context = context;
        }

        [HttpGet("{eventId}")]
        public async Task<IActionResult> GetRoute(int eventId)
        {
            var route = await _context.Routes
                .FirstOrDefaultAsync(r => r.EventID == eventId);

            if (route == null)
                return NotFound("Route not found.");

            return Ok(route);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoute(RaceDayRoute route)
        {
            if (!SessionHelper.IsOrganiser(HttpContext))
                return Unauthorized(
                    "Only organisers can create routes.");

            _context.Routes.Add(route);

            await _context.SaveChangesAsync();

            return Ok(route);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoute(
            int id,
            RaceDayRoute updatedRoute)
        {
            if (!SessionHelper.IsOrganiser(HttpContext))
                return Unauthorized(
                    "Only organisers can update routes.");

            var route = await _context.Routes
                .FirstOrDefaultAsync(r => r.RouteID == id);

            if (route == null)
                return NotFound("Route not found.");

            route.RouteName = updatedRoute.RouteName;
            route.DistanceKm = updatedRoute.DistanceKm;
            route.ElevationGainM = updatedRoute.ElevationGainM;
            route.StartPoint = updatedRoute.StartPoint;
            route.FinishPoint = updatedRoute.FinishPoint;
            route.RouteUrl = updatedRoute.RouteUrl;

            await _context.SaveChangesAsync();

            return Ok(route);
        }
    }
}