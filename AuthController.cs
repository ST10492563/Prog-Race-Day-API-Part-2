using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayApi.Data;
using RaceDayApi.DTOs;
using RaceDayApi.Models;

namespace RaceDayApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public AuthController(RaceDayDbContext context)
        {
            _context = context;
        }


        // REGISTER
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto request)
        {
            // Check whether email already exists

            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (existingUser != null)
            {
                return BadRequest("Email is already registered.");
            }


            // Check role

            if (request.Role != "Participant" &&
                request.Role != "Organiser")
            {
                return BadRequest(
                    "Role must be Participant or Organiser.");
            }


            // Hash password

            string passwordHash =
                BCrypt.Net.BCrypt.HashPassword(request.Password);


            // Create user

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PasswordHash = passwordHash,
                Phone = request.Phone,
                Role = request.Role,
                CreatedAt = DateTime.Now
            };


            // Add to database

            _context.Users.Add(user);

            await _context.SaveChangesAsync();


            return Ok(new
            {
                message = "Registration successful.",
                userID = user.UserID,
                name = user.FirstName + " " + user.LastName,
                email = user.Email,
                role = user.Role
            });
        }


        // LOGIN
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                return Unauthorized(
                    "Invalid email or password.");
            }


            // Check password

            bool passwordCorrect =
                BCrypt.Net.BCrypt.Verify(
                    request.Password,
                    user.PasswordHash);


            if (!passwordCorrect)
            {
                return Unauthorized(
                    "Invalid email or password.");
            }


            // Create session

            HttpContext.Session.SetInt32(
                "UserID",
                user.UserID);

            HttpContext.Session.SetString(
                "Role",
                user.Role);


            return Ok(new
            {
                message = "Login successful.",
                userID = user.UserID,
                name = user.FirstName + " " + user.LastName,
                email = user.Email,
                role = user.Role
            });
        }


        // LOGOUT
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return Ok(new
            {
                message = "Logout successful."
            });
        }
    }
}