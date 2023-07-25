using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalSurveysApp.Models
{
    public class Complaint
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!;
        [Required]
        public int DepartmentName { get; set; }
        [Required]
        public RequestStatus Status { get; set; } = RequestStatus.Pending;

        [ForeignKey("UserId")]
        public int UserId { get; set; }
    }
}
