using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Dtos.AttendanceDtos.LocationDto
{
    public class UpdateLocationRequestDto
    {
        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^[\u0621-\u064A\s]*$", ErrorMessage = "Invalid input")]
        public string NameAr { get; set; } = null!;
        [Required]
        [RegularExpression(@"^[a-zA-Z' -]*$", ErrorMessage = "Invalid input")]
        public string NameEn { get; set; } = null!;

        [Required]
        [RegularExpression(@"^(-?\d{1,3}(?:\.\d+)?)$", ErrorMessage = "Invalid input")]
        public double Longitude { get; set; }

        [Required]
        [RegularExpression(@"^(-?\d{1,2}(?:\.\d+)?)$", ErrorMessage = "Invalid input")]
        public double Latitude { get; set; }
    }
}
