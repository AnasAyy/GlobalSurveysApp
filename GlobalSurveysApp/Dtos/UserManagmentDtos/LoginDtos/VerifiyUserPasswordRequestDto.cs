using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Dtos.UserManagmentDtos.LoginManagement
{
    public class VerifiyUserPasswordRequestDto
    {
      
        [RegularExpression(@"^(?=^.{8,}$)(?=.*\d)(?=.*[a-zA-Z])[0-9a-zA-Z]*$", ErrorMessage = "Invalid Input")]
        public string Password { get; set; } = null!;

        [RegularExpression(@"^(?=^.{8,}$)(?=.*\d)(?=.*[a-zA-Z])[0-9a-zA-Z]*$", ErrorMessage = "Invalid Input")]

        public string ConfirmPassword { get; set; } = null!;

        public string SerialNumber = null!;
    }
}
