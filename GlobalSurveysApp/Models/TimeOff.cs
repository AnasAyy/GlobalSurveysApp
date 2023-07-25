﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalSurveysApp.Models
{
    public class TimeOff
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Type { get; set; }
        [Required]
        public DateTime From { get; set; }
        
        [Required]
        public DateTime To { get; set; }
        [Required]
        public int Number { get; set; }
        [Required]
        public string EmergencyNumber { get; set; } = null!;
        [Required]
        public DateTime CreatedAt { get; set;} = DateTime.Now;
        [Required]
        public RequestStatus Status { get; set; } = RequestStatus.Pending;
        [Required]
        public int UserId { get; set; }
        [Required]
        public int SubstituteEmployeeId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
        [ForeignKey("SubstituteEmployeeId")]
        public virtual User SubstituteEmployee { get; set; } = null!;


    }
}
