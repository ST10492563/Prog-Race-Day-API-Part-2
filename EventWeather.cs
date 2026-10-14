using System.ComponentModel.DataAnnotations;

namespace RaceDayApi.Models
{
    public class EventWeather
    {
        [Key]
        public int WeatherID { get; set; }

        [Required]
        public int EventID { get; set; }

        public DateTime ForecastDate { get; set; }

        public decimal TemperatureC { get; set; }

        [MaxLength(80)]
        public string? Condition { get; set; }

        public decimal WindSpeedKmh { get; set; }

        public int RainChancePercent { get; set; }

        public DateTime RecordedAt { get; set; } = DateTime.Now;

        public Event? Event { get; set; }
    }
}