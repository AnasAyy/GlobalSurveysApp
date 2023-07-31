using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Models
{
    public class PublicList
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string NameAR { get; set; } = null!;
        [Required]
        public string NameEN { get; set; } = null!;
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; }
        [Required]
        public int Type { get; set; } = 0;
    }
}
