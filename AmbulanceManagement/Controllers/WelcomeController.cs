using AmbulanceManagement.Data;
using AmbulanceManagement.Models;
using AmbulanceManagement.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Net.Mail;

namespace AmbulanceManagement.Controllers
{
    public class WelcomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public WelcomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var doctors = _userManager.GetUsersInRoleAsync("Doctor").Result;
            ViewData["DoctorData"] = doctors;
            ViewData["DoctorId"] = new SelectList(doctors, "Id", "Name");
            var errorMessage = TempData["ErrorMessage"] as string;
            ViewBag.ErrorMessage = errorMessage;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> MakeAppointment(AppointmentReqViewModel model)
        {


            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid input. Please check your input and try again.";
                return RedirectToAction("Index", "Welcome");
            }

            // Check if the patient with the provided email address exists
            var result = _context.Patient.FirstOrDefault(x => x.EmailAddress == model.Email);

            if (result == null)
            {
                TempData["ErrorMessage"] = "Patient not found with the provided email address.";
                return RedirectToAction("Index", "Welcome");
            }

            // Create the appointment
            var app = new Appointment
            {
                PatientId = result.Id,
                DoctorId = model.DoctorId,
                AppointmentDate = model.Date,
                AppointmentHour = model.Hour,
                IsApproved = false
            };

            try
            {
                // Add the appointment to the context and save changes
                _context.Add(app);
                await _context.SaveChangesAsync();

                // Redirect to the "Index" action of the "Welcome" controller
                return RedirectToAction("Index", "Welcome");
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle the concurrency exception (e.g., show an error message or retry the operation)
                TempData["ErrorMessage"] = "Concurrency error occurred while saving the appointment.";
                return RedirectToAction("Index", "Welcome");
            }
        }

    }
}
