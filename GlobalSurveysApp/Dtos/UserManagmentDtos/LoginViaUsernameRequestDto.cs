using System.ComponentModel.DataAnnotations;

namespace  GlobalSurveysApp.Dtos.UserManagmentDtos
{
    public class LoginViaUsernameRequestDto
    {
        [Required]
        [RegularExpression(@"^(?=.{2,30}$)[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z]+(?:\s[\u0600-\u065F\u066A-\u06EF\u06FA - \u06FFa - zA - Z]+)?$", ErrorMessage = "Invalid Input")]
        public string Username { get; set; } = null!;

        [Required]
        [RegularExpression(@"^(?=^.{8,}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$", ErrorMessage = "Invalid Input")]
        public string Password { get; set; } = null!;
    }
}
