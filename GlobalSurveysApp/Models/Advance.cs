using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalSurveysApp.Models
{
    public class Advance
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Amoount { get; set; }
        [Required]
        public DateTime From { get; set; }
        [Required]
        public DateTime To { get; set; }
        [Required]
        public DateTime CreateAt { get; set; }
        [Required]
        public RequestStatus Status { get; set; }
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
