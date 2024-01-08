using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmbulanceManagement.Models
{
    public class Report

    {
        [Key]
        public int ReportId { get; set; }
        public int AppointmentId { get; set; }
        public string? DoctorId { get; set; }
        public DateTime VisitDate { get; set; }
        public string? Symptoms { get; set; }
        public string? Diagnosis { get; set; }
        public string? Prescriptions { get; set; }

        [ForeignKey("DoctorId")]
        public virtual ApplicationUser? Doctor { get; set; }

        [ForeignKey("AppointmentId")]
        public virtual Appointment? Appointment { get; set; }
    }
}
