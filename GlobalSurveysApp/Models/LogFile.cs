using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalSurveysApp.Models
{
    public class LogFile
    {
        [Key]
        public int Id { get; set; }
        public string Action { get; set; } = string.Empty;
        public string ActionDescription { get; set; } = string.Empty;
        [Required]
        public int LogType { get; set; }
        public string IpAddress { get; set; } = string.Empty;
        public string SessionId { get; set; } = string.Empty;
        [Required]
        public string AppName { get; set; } = "GS App";
        public int LogMessageId { get; set; }
        
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }
}
