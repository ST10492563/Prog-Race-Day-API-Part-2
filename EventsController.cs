using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayApi.Data;
using RaceDayApi.Models;
using RaceDayApi.Security;

namespace RaceDayApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public EventsController(RaceDayDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            var events = await _context.Events
                .Include(e => e.Organiser)
                .ToListAsync();

            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvent(int id)
        {
            var eventItem = await _context.Events
                .Include(e => e.Organiser)
                .FirstOrDefaultAsync(e => e.EventID == id);

            if (eventItem == null)
                return NotFound("Event not found.");

            return Ok(eventItem);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent(Event eventItem)
        {
            if (!SessionHelper.IsOrganiser(HttpContext))
                return Unauthorized("Only organisers can create events.");

            eventItem.OrganiserID =
                SessionHelper.GetUserID(HttpContext)!.Value;

            _context.Events.Add(eventItem);

            await _context.SaveChangesAsync();

            return Ok(eventItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(
            int id,
            Event updatedEvent)
        {
            if (!SessionHelper.IsOrganiser(HttpContext))
                return Unauthorized("Only organisers can update events.");

            var existingEvent = await _context.Events
                .FirstOrDefaultAsync(e => e.EventID == id);

            if (existingEvent == null)
                return NotFound("Event not found.");

            existingEvent.EventName = updatedEvent.EventName;
            existingEvent.Description = updatedEvent.Description;
            existingEvent.EventDate = updatedEvent.EventDate;
            existingEvent.StartTime = updatedEvent.StartTime;
            existingEvent.Location = updatedEvent.Location;
            existingEvent.Province = updatedEvent.Province;
            existingEvent.EntryFee = updatedEvent.EntryFee;
            existingEvent.MaxParticipants = updatedEvent.MaxParticipants;
            existingEvent.Status = updatedEvent.Status;

            await _context.SaveChangesAsync();

            return Ok(existingEvent);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            if (!SessionHelper.IsOrganiser(HttpContext))
                return Unauthorized("Only organisers can delete events.");

            var eventItem = await _context.Events
                .FirstOrDefaultAsync(e => e.EventID == id);

            if (eventItem == null)
                return NotFound("Event not found.");

            _context.Events.Remove(eventItem);

            await _context.SaveChangesAsync();

            return Ok("Event deleted successfully.");
        }
    }
}