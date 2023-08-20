namespace GlobalSurveysApp.Dtos.ComplaintDtos
{
    public class GetComplaintForApproverResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime Date { get; set; }
        public int Title { get; set; }
        public string TypeAR { get; set; } = null!;
        public string TypeEN { get; set; } = null!;
    }
}
