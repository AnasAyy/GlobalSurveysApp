using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using GlobalSurveysApp.Models;

namespace GlobalSurveysApp.Dtos.UserManagmentDtos.LoginManagement
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = null!;
        public UserResponceDto User { get; set; } = null!;

    }
    public class UserResponceDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string UserRole { get; set; } = null!;
        public bool IsVerified { get; set; }
        public string Department { get; set; } = null!;

    }
}
