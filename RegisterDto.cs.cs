using System.ComponentModel.DataAnnotations;

namespace RaceDayApi.DTOs
{
    public class RegisterDto
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(120)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [MaxLength(25)]
        public string? Phone { get; set; }

        [Required]
        public string Role { get; set; } = "Participant";
    }
}