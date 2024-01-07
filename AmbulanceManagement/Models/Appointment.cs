using AmbulanceManagement.Utility;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace AmbulanceManagement.Models
{
    public class Appointment
    {
            [Key]
            public int AppointmentId { get; set; }
            public int? PatientId { get; set; }
            public string? DoctorId { get; set; }

            [DataType(DataType.Date)]
            public DateTime AppointmentDate { get; set; } 
            
            public Hour AppointmentHour { get; set; }
            public bool IsApproved { get; set; }

            [ForeignKey("DoctorId")]
            public virtual ApplicationUser? Doctor { get; set; }
            [ForeignKey("PatientId")]
            public virtual Patient? Patient { get; set; }
            public virtual Report? Report { get; set; }

    }
}
