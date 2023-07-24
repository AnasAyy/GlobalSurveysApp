using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!;
        [Required]
        public bool IsActived { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        public ICollection<User> Users { get; set; } = null!;
    }
}
