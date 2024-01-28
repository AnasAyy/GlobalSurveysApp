using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Models
{
    public class Location
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        [Required]
        public double Longitude { get; set; }
        [Required]
        public double Latitude { get; set; }
        
        public DateTime CreatedAt {  get; set; } = DateTime.Now;
        public DateTime UpdateddAt {  get; set; }
        public ICollection<User> Users { get; set; } = null!;

    }
}
