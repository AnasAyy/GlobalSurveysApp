using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Dtos.UserManagmentDtos
{
    public class LoginViaQRcodeRequestDto
    {
        [Required]
        //[RegularExpression(@"^[A-z]{2}[0-9]{5},|,|[0-9]{4}|\d{1,2}\/\d{1,2}\/\d{0}|\s..|[,.*?:/]\d.{1,}", ErrorMessage = "Invalid Input")]
        public string QRcode { get; set; } = null!;

    }
}
