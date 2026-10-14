using System.ComponentModel.DataAnnotations;

namespace RaceDayApi.Models
{
    public class Result
    {
        [Key]
        public int ResultID { get; set; }

        [Required]
        public int EnrollmentID { get; set; }

        public TimeSpan? FinishTime { get; set; }

        public int? Position { get; set; }

        [MaxLength(20)]
        public string? AveragePace { get; set; }

        public string ResultStatus { get; set; } = "Finished";

        public EventEnrollment? Enrollment { get; set; }
    }
}