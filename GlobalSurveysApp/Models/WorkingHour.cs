using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Models
{
    public class WorkingHour
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Start { get; set; } = null!;
        [Required]
        public string End { get; set; } = null!;
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdateddAt { get; set; }

        public ICollection<User> Users { get; set; } = null!;
    }
}
