namespace GlobalSurveysApp.Dtos.ComplaintDtos
{
    public class GetComplaintForApproverByNameDto
    {
        public int Page { get; set; }
        public string Name { get; set; } = null!;
    }
}
