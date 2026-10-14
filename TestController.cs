using Microsoft.AspNetCore.Mvc;
using RaceDayApi.Security;

namespace RaceDayApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet("session")]
        public IActionResult TestSession()
        {
            if (!SessionHelper.IsLoggedIn(HttpContext))
            {
                return Unauthorized("You are not logged in.");
            }

            return Ok(new
            {
                userID = SessionHelper.GetUserID(HttpContext),
                role = SessionHelper.GetRole(HttpContext),
                isOrganiser = SessionHelper.IsOrganiser(HttpContext),
                isParticipant = SessionHelper.IsParticipant(HttpContext)
            });
        }
    }
}