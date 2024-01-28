using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Dtos.UserManagmentDtos.UserDtos
{
    public class UpdateUserRequestDto
    {

        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int Id { get; set; }

        [RegularExpression(@"^[\p{L}\p{M}' \.\-]+$", ErrorMessage = "Invalid Input")]
        public string FirstName { get; set; } = null!;


        [RegularExpression(@"^[\p{L}\p{M}' \.\-]+$", ErrorMessage = "Invalid Input")]
        public string SecondName { get; set; } = null!;


        [RegularExpression(@"^[\p{L}\p{M}' \.\-]+$", ErrorMessage = "Invalid Input")]
        public string ThirdName { get; set; } = null!;


        [RegularExpression(@"^[\p{L}\p{M}' \.\-]+$", ErrorMessage = "Invalid Input")]
        public string LastName { get; set; } = null!;


        [RegularExpression(@"^\+\d+$", ErrorMessage = "Invalid Input")]
        public string WorkMobile { get; set; } = null!;


        [RegularExpression(@"^\+\d+$", ErrorMessage = "Invalid Input")]
        public string PrivateMobile { get; set; } = null!;

        

        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int PlaceOfBirth { get; set; }


        public DateTime DateOfBirth { get; set; }


        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int CertificateLevel { get; set; }


        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int FieldOfStudy { get; set; }


        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int PassportNumber { get; set; }


        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int Gender { get; set; }


        public DateTime FirstContractDate { get; set; }


        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int Postion { get; set; }


        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int Nationality { get; set; }


        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = null!;


        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int Department { get; set; }


        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int Location { get; set; }

        public IFormFile? IdCard { get; set; }
        public string? QRcode { get; set; } = null!;//

        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int? DirectResponsibleId { get; set; }

        [RegularExpression(@"^[\p{L}\p{M}' \.\-]+$", ErrorMessage = "Invalid Input")]
        public bool Status { get; set; }//

        [RegularExpression(@"^[a-zA-Z0-9+]{8,}$", ErrorMessage = "Invalid Input")]
        public string? Password { get; set; } = null!;//

        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int RoleId { get; set; }

       
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int LocationId { get; set; }

       
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int WorkingHourId { get; set; }

        
        public List<int> WorkingDays { get; set; } = null!;

        
        public string SerialNumber { get; set; } = null!;

    }
}
