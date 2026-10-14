using System.ComponentModel.DataAnnotations;

namespace RaceDayApi.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [MaxLength(120)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [MaxLength(25)]
        public string? Phone { get; set; }

        [Required]
        public string Role { get; set; } = "Participant";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
