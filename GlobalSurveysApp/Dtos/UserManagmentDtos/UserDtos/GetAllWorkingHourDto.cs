using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Dtos.UserManagmentDtos.UserDtos
{
    public class GetAllWorkingHourDto
    {
        public int Id { get; set; }
        public string Time { get; set; } = null!;
    }
}
