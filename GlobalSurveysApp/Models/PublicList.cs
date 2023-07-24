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
        public string NameEn { get; set; } = null!;
        [Required]
        public string Type { get; set; } = null!;
    }
}
