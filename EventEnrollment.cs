using System.ComponentModel.DataAnnotations;

namespace RaceDayApi.Models
{
    public class EventEnrollment
    {
        [Key]
        public int EnrollmentID { get; set; }

        [Required]
        public int EventID { get; set; }

        [Required]
        public int ParticipantID { get; set; }

        [Required]
        public int CategoryID { get; set; }

        public DateTime EnrollmentDate { get; set; } = DateTime.Now;

        [MaxLength(20)]
        public string? RaceNumber { get; set; }

        [Required]
        public string Status { get; set; } = "Registered";

        public Event? Event { get; set; }

        public User? Participant { get; set; }

        public Category? Category { get; set; }

        public Result? Result { get; set; }
    }
}