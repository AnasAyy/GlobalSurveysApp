using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalSurveysApp.Models
{
    public class Complaint
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Title { get; set; }
        [Required]
        public string Description { get; set; } = null!;
        [Required]
        public string Against { get; set; } = null!;
        [Required]
        public bool IsUpdated { get; set; } = false;
        [Required]
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        public RequestStatus Status { get; set; } = RequestStatus.Pending;

        [ForeignKey("UserId")]
        public int UserId { get; set; }
    }
}
