﻿using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Dtos.AdvanceDtos
{
    public class GetUserAdvanceRequestDto
    {
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int Page { get; set; }
    }
}
