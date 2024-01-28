using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Models
{
    public class WorkingDay
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdateddAt { get; set; }

        

    }
}
