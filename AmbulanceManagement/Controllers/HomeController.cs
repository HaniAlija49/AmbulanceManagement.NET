using AmbulanceManagement.Data;
using AmbulanceManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AmbulanceManagement.Controllers
{
	public class HomeController : Controller
	{
        private readonly ApplicationDbContext _db;
		private readonly UserManager<ApplicationUser> _userManager;

        public HomeController( ApplicationDbContext db,UserManager<ApplicationUser> userManager)
		{

			_db = db;
			_userManager = userManager;
		}
        public IActionResult Index()
		{
            var doctors = _userManager.GetUsersInRoleAsync("Doctor").Result;
            ViewData["DoctorData"] = doctors;
			ViewData["PatientData"] = _db.Patient.ToList();
			ViewData["AppointmentsData"] = _db.Appointment.ToList();

            return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}