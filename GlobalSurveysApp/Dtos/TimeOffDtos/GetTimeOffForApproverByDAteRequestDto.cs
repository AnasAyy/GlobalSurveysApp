namespace GlobalSurveysApp.Dtos.TimeOffDtos
{
    public class GetTimeOffForApproverByDAteRequestDto
    {
        public int Page { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
