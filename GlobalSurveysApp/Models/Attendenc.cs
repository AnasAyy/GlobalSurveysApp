using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalSurveysApp.Models
{
    public class Attendenc
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public TimeSpan CheckIn { get; set; }
        public TimeSpan CheckOut { get; set; }
        [ForeignKey("UserId")]
        public int UserId { get; set; }
    }
}
