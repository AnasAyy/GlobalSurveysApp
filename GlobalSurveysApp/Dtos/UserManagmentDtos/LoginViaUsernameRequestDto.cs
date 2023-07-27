using System.ComponentModel.DataAnnotations;

namespace  GlobalSurveysApp.Dtos.UserManagmentDtos
{
    public class LoginViaUsernameRequestDto
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]{3,30}$", ErrorMessage = "Invalid Input")]
        public string Username { get; set; } = null!;

        [Required]
        [RegularExpression(@"^(?=^.{8,}$)(?=.*\d)(?=.*[a-zA-Z])[0-9a-zA-Z]*$", ErrorMessage = "Invalid Input")]
        public string Password { get; set; } = null!;
    }
}
