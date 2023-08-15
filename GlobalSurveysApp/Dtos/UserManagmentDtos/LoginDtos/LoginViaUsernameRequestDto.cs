using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Dtos.UserManagmentDtos.LoginManagement
{
    public class LoginViaUsernameRequestDto
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9+]{3,30}$", ErrorMessage = "Invalid Input")]
        public string Username { get; set; } = null!;

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9+]{8,}$", ErrorMessage = "Invalid Input")]
        public string Password { get; set; } = null!;
    }
}
