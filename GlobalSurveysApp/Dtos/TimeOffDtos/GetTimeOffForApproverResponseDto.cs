namespace GlobalSurveysApp.Dtos.TimeOffDtos
{
    public class GetTimeOffForApproverResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime Date { get; set; }
        public int Number { get; set; }
        public string TypeAR { get; set; } = null!;
        public string TypeEN { get; set; } = null!;
    }
}

