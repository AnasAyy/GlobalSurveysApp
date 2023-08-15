using GlobalSurveysApp.Dtos.PublicListDtos;
using GlobalSurveysApp.Models;
using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Dtos.UserManagmentDtos.UserDtos
{
    public class ViewUserResponceDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;


        public string SecondName { get; set; } = null!;


        public string ThirdName { get; set; } = null!;


        public string LastName { get; set; } = null!;


        public string WorkMobile { get; set; } = null!;


        public string PrivateMobile { get; set; } = null!;

        

        public int PlaceOfBirth { get; set; } 



        public DateTime DateOfBirth { get; set; }


        public int CertificateLevel { get; set; }


        public int FieldOfStudy { get; set; }


        public int PassportNumber { get; set; } 


        public int Gender { get; set; } 


        public DateTime FirstContractDate { get; set; }


        public int Position { get; set; } 


        public int Nationality { get; set; } 


        public string Email { get; set; } = null!;


        public int Department { get; set; }


        public int Location { get; set; } 


        public int? DirectResponsibleId { get; set; }

        public bool Status { get; set; }

        public string Role { get; set; }= null!;

        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public DateTime? LastLogIn { get; set; }
    }

    

}
