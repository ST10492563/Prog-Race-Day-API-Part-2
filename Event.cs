using System.ComponentModel.DataAnnotations;

namespace RaceDayApi.Models
{
    public class Event
    {
        [Key]
        public int EventID { get; set; }

        [Required]
        public int OrganiserID { get; set; }

        [Required]
        [MaxLength(150)]
        public string EventName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        [MaxLength(150)]
        public string Location { get; set; } = string.Empty;

        [Required]
        [MaxLength(80)]
        public string Province { get; set; } = string.Empty;

        public decimal EntryFee { get; set; }

        public int MaxParticipants { get; set; }

        public string Status { get; set; } = "Upcoming";

        public User? Organiser { get; set; }

        public ICollection<EventCategory> EventCategories { get; set; }
            = new List<EventCategory>();

        public ICollection<Route> Routes { get; set; }
            = new List<Route>();

        public ICollection<EventWeather> Weather { get; set; }
            = new List<EventWeather>();

        public ICollection<EventEnrollment> EventEnrollments { get; set; }
            = new List<EventEnrollment>();
    }
}