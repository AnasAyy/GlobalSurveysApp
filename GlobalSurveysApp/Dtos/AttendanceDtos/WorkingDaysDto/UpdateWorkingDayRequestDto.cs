﻿using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Dtos.AttendanceDtos.WorkingHourDto
{
    public class UpdateWorkingDayRequestDto
    {
        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^[\u0621-\u064A\s]*$", ErrorMessage = "Invalid input")]
        public string NameAr { get; set; } = null!;
        [Required]
        [RegularExpression(@"^[a-zA-Z -]*$", ErrorMessage = "Invalid input")]
        public string NameEn { get; set; } = null!;





    }
}
