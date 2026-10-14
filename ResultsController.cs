using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayApi.Data;
using RaceDayApi.Models;
using RaceDayApi.Security;

namespace RaceDayApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResultsController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public ResultsController(RaceDayDbContext context)
        {
            _context = context;
        }

        [HttpGet("{eventId}")]
        public async Task<IActionResult> GetResults(int eventId)
        {
            var results = await _context.Results
                .Include(r => r.Enrollment)
                .Where(r => r.Enrollment!.EventID == eventId)
                .OrderBy(r => r.Position)
                .ToListAsync();

            return Ok(results);
        }

        [HttpPost]
        public async Task<IActionResult> AddResult(Result result)
        {
            if (!SessionHelper.IsOrganiser(HttpContext))
                return Unauthorized(
                    "Only organisers can add results.");

            var enrollmentExists =
                await _context.EventEnrollments
                    .AnyAsync(e =>
                        e.EnrollmentID == result.EnrollmentID);

            if (!enrollmentExists)
                return NotFound("Enrollment not found.");

            _context.Results.Add(result);

            await _context.SaveChangesAsync();

            return Ok(result);
        }
    }
}
