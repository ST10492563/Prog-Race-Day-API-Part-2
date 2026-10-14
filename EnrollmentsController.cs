using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayApi.Data;
using RaceDayApi.Models;
using RaceDayApi.Security;

namespace RaceDayApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public EnrollmentsController(RaceDayDbContext context)
        {
            _context = context;
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyEnrollments()
        {
            if (!SessionHelper.IsParticipant(HttpContext))
                return Unauthorized(
                    "Only participants can view their enrollments.");

            int userID =
                SessionHelper.GetUserID(HttpContext)!.Value;

            var enrollments = await _context.EventEnrollments
                .Where(e => e.ParticipantID == userID)
                .Include(e => e.Event)
                .Include(e => e.Category)
                .Include(e => e.Result)
                .ToListAsync();

            return Ok(enrollments);
        }

        [HttpPost]
        public async Task<IActionResult> Enroll(
            EventEnrollment enrollment)
        {
            if (!SessionHelper.IsParticipant(HttpContext))
                return Unauthorized(
                    "Only participants can register.");

            int userID =
                SessionHelper.GetUserID(HttpContext)!.Value;

            var eventExists = await _context.Events
                .AnyAsync(e => e.EventID == enrollment.EventID);

            if (!eventExists)
                return NotFound("Event not found.");

            var categoryExists = await _context.Categories
                .AnyAsync(c => c.CategoryID == enrollment.CategoryID);

            if (!categoryExists)
                return NotFound("Category not found.");

            var alreadyRegistered =
                await _context.EventEnrollments.AnyAsync(e =>
                    e.EventID == enrollment.EventID &&
                    e.ParticipantID == userID);

            if (alreadyRegistered)
                return BadRequest(
                    "You are already registered for this event.");

            enrollment.ParticipantID = userID;
            enrollment.EnrollmentDate = DateTime.Now;
            enrollment.Status = "Registered";

            _context.EventEnrollments.Add(enrollment);

            await _context.SaveChangesAsync();

            return Ok(enrollment);
        }
    }
}