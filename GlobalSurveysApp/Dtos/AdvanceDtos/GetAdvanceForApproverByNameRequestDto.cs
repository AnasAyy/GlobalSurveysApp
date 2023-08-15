namespace GlobalSurveysApp.Dtos.AdvanceDtos
{
    public class GetAdvanceForApproverByNameRequestDto
    {
        public int Page { get; set; }
        public string Name { get; set; } = null!;
    }
}
