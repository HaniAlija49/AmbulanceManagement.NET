﻿using AmbulanceManagement.Utility;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace AmbulanceManagement.Models
{
    public class Appointment
    {
            [Key]
            public int AppointmentId { get; set; }
            public int PatientId { get; set; }
            public int DoctorId { get; set; }
            [DataType(DataType.Date)]
            [NotMapped]
            public DateOnly AppointmentDate { get; set; } 
            
            public Hour AppointmentHour { get; set; }
            public bool IsApproved { get; set; }
           
            //public virtual Patient Patient { get; set; }
           // public virtual Doctor Doctor { get; set; }

    }
}