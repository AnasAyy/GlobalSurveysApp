namespace GlobalSurveysApp.Dtos.AdvanceDtos
{
    public class GetAdvanceForApproverResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime Date { get; set; }
        public int Amount { get; set; }
        public string TypeAR { get; set; } = null!;
        public string TypeEN { get; set; } = null!;
    }
}
