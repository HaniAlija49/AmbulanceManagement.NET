using AmbulanceManagement.Models;
using AmbulanceManagement.Utility;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmbulanceManagement.ViewModels
{
    public class AppointmentReqViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string? DoctorId { get; set; }
        public Hour Hour { get; set; }
        public DateTime Date { get; set; }
        [ForeignKey("DoctorId")]
        public ApplicationUser ?Doctor { get; set; }
    }
}
