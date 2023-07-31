using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalSurveysApp.Models
{
    public class FCMtoken
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Token { get; set; } = null!;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;
        [ForeignKey("UserId")]
        public int UserId { get; set; }
    }
}
