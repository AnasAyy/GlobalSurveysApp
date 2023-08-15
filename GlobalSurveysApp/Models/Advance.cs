using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalSurveysApp.Models
{
    public class Advance
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public int Currency { get; set; }
        [Required]
        public DateTime From { get; set; }
        [Required]
        public DateTime To { get; set; }
        [Required]
        public DateTime CreateAt { get; set; }
        [Required]
        public bool IsUpdated { get; set; } = false;
        public DateTime? UpdatedAt { get; set; }
        public RequestStatus Status { get; set; } = RequestStatus.Pending;
        [ForeignKey("UserId")]
        public int UserId { get; set; }

    }

    public enum RequestStatus
    {
        Pending,
        Accepted,
        Rejected,
    }
}
