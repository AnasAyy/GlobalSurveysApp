using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Dtos.TimeOffDtos
{
    public class ApproveBySumEmpRequestDtos
    {
        [Required(ErrorMessage = "RequestId is required")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int RequestId { get; set; }

        [Required(ErrorMessage = "status is required")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int Status { get; set; }
    }
}
