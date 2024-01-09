using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AmbulanceManagement.Data;
using AmbulanceManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using AmbulanceManagement.Utility;

namespace AmbulanceManagement.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AppointmentsController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Appointment.Include(a => a.Doctor).Include(a => a.Patient);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Appointment == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(m => m.AppointmentId == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // GET: Appointments/Create
        public IActionResult Create()
        {
            ViewData["DoctorId"] = new SelectList(_context.Users, "Id", "Name");
            ViewData["PatientId"] = new SelectList(_context.Patient, "Id", "Name");
            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppointmentId,PatientId,DoctorId,AppointmentDate,AppointmentHour,IsApproved")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                // Check if the selected appointment slot is available
                var existingAppointment = await _context.Appointment
                    .FirstOrDefaultAsync(a => a.AppointmentDate == appointment.AppointmentDate && a.AppointmentHour == appointment.AppointmentHour);

                if (existingAppointment != null)
                {
                    // Slot is already taken, handle accordingly (e.g., display an error message)
                    ModelState.AddModelError("AppointmentHour", "Selected slot is already taken. Please choose another slot.");
                    ViewData["DoctorId"] = new SelectList(_context.Users, "Id", "Name", appointment.DoctorId);
                    ViewData["PatientId"] = new SelectList(_context.Patient, "Id", "Name", appointment.PatientId);
                    return View(appointment);
                }

                // Slot is available, proceed with saving the appointment
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If ModelState is not valid, populate dropdowns and return the view with the model
            ViewData["DoctorId"] = new SelectList(_context.Users, "Id", "Name", appointment.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patient, "Id", "Name", appointment.PatientId);
            return View(appointment);
        }
        private List<Hour> GetFreeHoursPrivate(DateTime appointmentDate)
        {
            var existingAppointments = _context.Appointment
                .Where(a => a.AppointmentDate == appointmentDate)
                .Select(a => a.AppointmentHour)
                .ToList();

            var allHours = Enum.GetValues(typeof(Hour)).Cast<Hour>().ToList();

            var freeHours = allHours.Except(existingAppointments).ToList();

            return freeHours;
        }

        [HttpGet]
        public JsonResult GetFreeHours(DateTime appointmentDate)
        {
            var freeHours = GetFreeHoursPrivate(appointmentDate);
            return Json(freeHours);
        }


        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Appointment == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            ViewData["DoctorId"] = new SelectList(_context.Users, "Id", "Name", appointment.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patient, "Id", "Name", appointment.PatientId);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AppointmentId,PatientId,DoctorId,AppointmentDate, AppointmentHour,IsApproved")] Appointment appointment)
        {
            if (id != appointment.AppointmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.AppointmentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DoctorId"] = new SelectList(_context.Users, "Id", "Name", appointment.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patient, "Id", "Name", appointment.PatientId);
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Appointment == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(m => m.AppointmentId == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Appointment == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Appointment'  is null.");
            }
            var appointment = await _context.Appointment.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointment.Remove(appointment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id)
        {
          return (_context.Appointment?.Any(e => e.AppointmentId == id)).GetValueOrDefault();
        }

        [HttpPost]
        public async Task<IActionResult> ToggleApproval(int id, bool approve)
        {
            var appointment = await _context.Appointment.FindAsync(id);
            string referringUrl = _httpContextAccessor.HttpContext.Request.Headers["Referer"].ToString();

            if (appointment == null)
            {
                return NotFound(); // Or handle the case where the appointment isn't found
            }

            appointment.IsApproved = approve;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(); // Handle the concurrency exception
            }

            
            return Redirect(referringUrl); // Redirect to appropriate action // Redirect to the appropriate action
        }

    }
}
