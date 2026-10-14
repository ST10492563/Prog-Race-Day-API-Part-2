using System.ComponentModel.DataAnnotations;

namespace RaceDayApi.Models
{
    public class Route
    {
        [Key]
        public int RouteID { get; set; }

        [Required]
        public int EventID { get; set; }

        [MaxLength(150)]
        public string? RouteName { get; set; }

        public decimal DistanceKm { get; set; }

        public int ElevationGainM { get; set; }

        [MaxLength(150)]
        public string? StartPoint { get; set; }

        [MaxLength(150)]
        public string? FinishPoint { get; set; }

        public string? RouteUrl { get; set; }

        public Event? Event { get; set; }
    }
}