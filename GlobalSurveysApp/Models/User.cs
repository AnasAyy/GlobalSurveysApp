using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

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
        public string WorkMobile { get; set; } = null!;
        [Required]

        public string PrivateMobile { get; set; } = null!;
        [Required]
        public int placeOfBirth { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public int CertificateLevel { get; set; }
        [Required]
        public int FieldOfStudy { get; set; }
        [Required]
        public int PassportNumber { get; set; }
        
        [Required]
        public int Gender { get; set; }
        [Required]
        public DateTime FirstContractDate { get; set; }
        [Required]
        public int Postion { get; set;}
        [Required]
        public int Nationality { get; set; }
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public int Department { get; set; } 
        [Required]
        public int Location { get; set; }
        
        [Required]
        public string UserName { get; set;} = null!;//
        [Required]
        public string Password { get; set; }= null!;//
        [Required] 
        public string QRcode { get; set; } = null!;

        public byte[]? IdCard { get; set; } = null!;
        public string? PersonalPhoto { get; set; }//
        [Required]
        public bool IsActive { get; set; } = true;
        [Required]
        public bool IsVerified { get; set; } = false;//
        public DateTime? LastLogin { get; set; }//
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;//
        public DateTime? UpdatedAt { get; set; }//
        public int? DirectResponsibleId { get; set; }
        [ForeignKey("DirectResponsibleId")]
        public virtual User DirectResponsible { get; set; } = null!;


        [ForeignKey("RoleId")]
        public int RoleId { get; set; }

        public ICollection<Advance> Advances { get; set; } = null!;
        public ICollection<Complaint> Complaints { get; set; } = null!;
        public ICollection<Message> Messages { get; set; } = null!; 
        public ICollection<Approver> Approvers { get; set; } = null!;
        public ICollection<FCMtoken> FCMtokens { get; set; } = null!;
        

    }
}
