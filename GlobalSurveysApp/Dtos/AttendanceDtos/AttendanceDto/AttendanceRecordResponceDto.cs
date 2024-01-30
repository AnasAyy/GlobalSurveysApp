namespace GlobalSurveysApp.Dtos.AttendanceDtos.AttendanceDto
{
    public class AttendanceRecordResponceDto
    {
        public DateTime Date { get; set; }
        public string Status { get; set; } = null!;
        public TimeSpan? CheckInTime { get; set; }
        public TimeSpan? CheckOutTime { get; set; }
        public TimeSpan? Daley { get; set; }
        
    }
}
