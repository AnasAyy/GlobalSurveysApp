using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalSurveysApp.Models
{
    public class Approver
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int RequestId { get; set; }
        [Required]
        public int RequestType { get; set; }
        [Required]
        public int ApproverType { get; set; }
        [Required]
        public RequestStatus Status { get; set; } = RequestStatus.Pending;
        
        public string Note { get; set; } = string.Empty;
        public DateTime UpdatedAt { get; set; }

        [ForeignKey("UserId")]
        public int UserId { get; set; }

    }
}
