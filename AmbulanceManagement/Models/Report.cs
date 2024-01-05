using System.ComponentModel.DataAnnotations.Schema;

namespace AmbulanceManagement.Models
{
    public class Report

    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime VisitDate { get; set; }
        public string Symptoms { get; set; }
        public string Diagnosis { get; set; }

        [ForeignKey("DoctorId")]
        public virtual ApplicationUser Doctor { get; set; }

        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }
    }
}
