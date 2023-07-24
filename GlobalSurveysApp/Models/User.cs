using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalSurveysApp.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string SecondName { get; set; } = null!;
        [Required]
        public string ThirdName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
        [Required]
        public string PhoneNumber { get; set; } = null!;
        [Required]
        public string Email { get; set; } = null!;
        [Required] 
        public string UserName { get; set;} = null!;
        [Required]
        public string Password { get; set; }= null!;
        [Required] 
        public string QRcode { get; set; } = null!;
        [Required]
        public bool IsActive { get; set; } = true;
        public DateTime? LastLogin { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public string FCMtoken { get; set; } = string.Empty;
        public ICollection<User> Users { get; set; } = null!;

        [ForeignKey("DirectResponsibleId")]
        public int DirectResponsibleId { get; set; }

        [ForeignKey("RoleId")]
        public int RoleId { get; set; }

        public ICollection<Advance> Advances { get; set; } = null!;
        public ICollection<Complaint> Complaints { get; set; } = null!;
        public ICollection<Message> Messages { get; set; } = null!;
        




    }
}
