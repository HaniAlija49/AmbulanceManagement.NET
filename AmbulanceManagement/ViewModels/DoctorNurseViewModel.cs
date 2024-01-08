using AmbulanceManagement.Models;

namespace AmbulanceManagement.ViewModels
{
    public class DoctorNurseViewModel
    {
        public List<ApplicationUser> DoctorUsers { get; set; }
        public List<ApplicationUser> NurseUsers { get; set; }
    }

}
