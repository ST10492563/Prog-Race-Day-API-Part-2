using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayApi.Data;
using RaceDayApi.Models;
using RaceDayApi.Security;

namespace RaceDayApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventCategoriesController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public EventCategoriesController(RaceDayDbContext context)
        {
            _context = context;
        }

        [HttpGet("{eventId}")]
        public async Task<IActionResult> GetEventCategories(int eventId)
        {
            var categories = await _context.EventCategories
                .Where(ec => ec.EventID == eventId)
                .Include(ec => ec.Category)
                .ToListAsync();

            return Ok(categories);
        }

        [HttpPost]
        public async Task<IActionResult> AddEventCategory(
            EventCategory eventCategory)
        {
            if (!SessionHelper.IsOrganiser(HttpContext))
                return Unauthorized(
                    "Only organisers can add event categories.");

            _context.EventCategories.Add(eventCategory);

            await _context.SaveChangesAsync();

            return Ok(eventCategory);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteEventCategory(
            int eventId,
            int categoryId)
        {
            if (!SessionHelper.IsOrganiser(HttpContext))
                return Unauthorized(
                    "Only organisers can remove event categories.");

            var item = await _context.EventCategories
                .FirstOrDefaultAsync(ec =>
                    ec.EventID == eventId &&
                    ec.CategoryID == categoryId);

            if (item == null)
                return NotFound("Event category not found.");

            _context.EventCategories.Remove(item);

            await _context.SaveChangesAsync();

            return Ok("Category removed from event.");
        }
    }
}