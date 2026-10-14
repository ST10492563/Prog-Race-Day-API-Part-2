using System.ComponentModel.DataAnnotations;

namespace RaceDayApi.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required]
        [MaxLength(80)]
        public string CategoryName { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Description { get; set; }

        public ICollection<EventCategory> EventCategories { get; set; }
            = new List<EventCategory>();
    }
}