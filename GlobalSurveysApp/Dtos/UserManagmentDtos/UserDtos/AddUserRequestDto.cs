using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Dtos.UserManagmentDtos.UserRequest
{
    public class AddUserRequestDto
    {
        [Required(ErrorMessage = "First name is required")]
        [RegularExpression(@"^[\p{L}\p{M}' \.\-]+$", ErrorMessage = "Invalid Input")]
        public string FirstName { get; set; } = null!;


        [Required(ErrorMessage = "Second name is required")]
        [RegularExpression(@"^[\p{L}\p{M}' \.\-]+$", ErrorMessage = "Invalid Input")]
        public string SecondName { get; set; } = null!;


        [Required(ErrorMessage = "Third name is required")]
        [RegularExpression(@"^[\p{L}\p{M}' \.\-]+$", ErrorMessage = "Invalid Input")]
        public string ThirdName { get; set; } = null!;


        [Required(ErrorMessage = "Last name is required")]
        [RegularExpression(@"^[\p{L}\p{M}' \.\-]+$", ErrorMessage = "Invalid Input")]
        public string LastName { get; set; } = null!;


        [Required(ErrorMessage = "Work number is required")]
        [RegularExpression(@"^\+\d+$", ErrorMessage = "Invalid Input")]
        public string WorkMobile { get; set; } = null!;


        [Required(ErrorMessage = "Private number is required")]
        [RegularExpression(@"^\+\d+$", ErrorMessage = "Invalid Input")]
        public string PrivateMobile { get; set; } = null!;

        [Required(ErrorMessage = "Identity Card is required")]
        public IFormFile IdCard { get; set; } = null!;

        [Required(ErrorMessage = "Place Of Birth is required")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int PlaceOfBirth { get; set; }


        [Required(ErrorMessage = "Birth date is required")]
        
        public DateTime DateOfBirth { get; set; }


        [Required(ErrorMessage = "Certificate Level is required")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int CertificateLevel { get; set; }


        [Required(ErrorMessage = "Field Of Study is required")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int FieldOfStudy { get; set; }


        [Required(ErrorMessage = "Passport Number is required")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int PassportNumber { get; set; } 


        [Required(ErrorMessage = "Gender is required")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int Gender { get; set; }


        [Required(ErrorMessage = "First Contract Date is required")]
        public DateTime FirstContractDate { get; set; }


        [Required(ErrorMessage = "Postion is required")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int Postion { get; set; }


        [Required(ErrorMessage = "Nationality is required")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int Nationality { get; set; }


        [Required(ErrorMessage = "Email address is required")]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = null!;


        [Required(ErrorMessage = "Department is required")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int Department { get; set; }


        [Required(ErrorMessage = "Location is required")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int Location { get; set; }


        public string QRcode { get; set; } = null!;


        public int? DirectResponsibleId { get; set; }


        [Required(ErrorMessage = "Role is required")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int RoleId { get; set; }
        
        [Required(ErrorMessage = "Location Id is required")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int LocationId { get; set; }
        
        [Required(ErrorMessage = "Working Hour Id is required")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int WorkingHourId { get; set; }

        [Required(ErrorMessage = "Working Days is required")]
        public List<int> WorkingDays { get; set; } = null!;


    }
}
