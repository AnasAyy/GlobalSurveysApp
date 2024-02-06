namespace GlobalSurveysApp.Dtos.AttendanceDtos.AttendanceDto
{
    public class AttendanceSummaryDto
    {
        public string UserName { get; set; } = null!;
        public int AbsentDays { get; set; }
        public int PresentDays { get; set; }
        public int TimeOffDays { get; set; }
        public TimeSpan TotalDelay { get; set; }
        public TimeSpan TotalWorkingHours { get; set; }
    }
}
