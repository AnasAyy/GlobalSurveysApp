namespace GlobalSurveysApp.Dtos.AttendanceDtos.AttendanceDto
{
    public class AttendanceSummariesDto
    {

       
            public int? TimeOffCount { get; set; }
            public int? PresentCount { get; set; }
            public int? AbsentCount { get; set; }
            public TimeSpan DelaySum { get; set; }
            public int? HolidayCount { get; set; }
            public TimeSpan WorkingHourSum { get; set; }
        

    }
}
