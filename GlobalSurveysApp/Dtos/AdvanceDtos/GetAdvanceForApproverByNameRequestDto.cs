using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Dtos.AdvanceDtos
{
    public class GetAdvanceForApproverByNameRequestDto
    {
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int Page { get; set; }
        public string Name { get; set; } = null!;
    }
}
