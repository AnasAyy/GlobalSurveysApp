using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Dtos.AttendanceDtos.LocationDto
{
    public class GetAllLocationsRequestDto
    {
        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int Page { get; set; }
    }
}
