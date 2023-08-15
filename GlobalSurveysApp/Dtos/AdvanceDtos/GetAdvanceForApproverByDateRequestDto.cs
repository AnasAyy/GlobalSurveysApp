namespace GlobalSurveysApp.Dtos.AdvanceDtos
{
    public class GetAdvanceForApproverByDateRequestDto
    {
        public int Page { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
