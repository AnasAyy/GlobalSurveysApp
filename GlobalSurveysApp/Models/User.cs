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
        public int Department { get; set; } 
        [Required]
        public int Location { get; set; }
        
        [Required]
        public string UserName { get; set;} = null!;
        [Required]
        public string Password { get; set; }= null!;
        [Required] 
        public string QRcode { get; set; } = null!;
        [Required]
        public bool IsActive { get; set; } = true;
        [Required]
        public bool IsVerified { get; set; } = false;
        public DateTime? LastLogin { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public string FCMtoken { get; set; } = string.Empty;
        public int? DirectResponsibleId { get; set; }
        [ForeignKey("DirectResponsibleId")]
        public virtual User DirectResponsible { get; set; } = null!;


        [ForeignKey("RoleId")]
        public int RoleId { get; set; }

        public ICollection<Advance> Advances { get; set; } = null!;
        public ICollection<Complaint> Complaints { get; set; } = null!;
        public ICollection<Message> Messages { get; set; } = null!; 
        public ICollection<Approver> Approvers { get; set; } = null!;
        




    }
}
