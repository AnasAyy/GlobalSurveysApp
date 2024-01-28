namespace GlobalSurveysApp.Dtos.AttendanceDtos.WorkingHourDto
{
    public class GetAllWorkingHoureResponseDto
    {
        public int Id { get; set; }
        public string Start { get; set; } = null!;
        public string End { get; set; } = null!;
    }
}
