using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AmbulanceManagement.Data;
using AmbulanceManagement.Models;
using AmbulanceManagement.Migrations;
using Microsoft.AspNetCore.Identity;

namespace AmbulanceManagement.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReportsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Reports
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Report.Include(r => r.Appointment).Include(r => r.Doctor);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Reports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Report == null)
            {
                return NotFound();
            }

            var report = await _context.Report
                .Include(r => r.Appointment)
                .Include(r => r.Doctor)
                .FirstOrDefaultAsync(m => m.ReportId == id);

           var patient = _context.Patient.FirstOrDefault(p => p.Id == report.Appointment.PatientId);


            ViewBag.PatientName = patient.Name;

            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        // GET: Reports/Create
        public IActionResult Create()
        {
            ViewData["AppointmentId"] = new SelectList(_context.Appointment, "AppointmentId", "AppointmentId");
            ViewData["DoctorId"] = new SelectList(_context.Users, "Id", "Name");
            return View();
        }


        public IActionResult CreateWithId(int appointmentId, string doctorId, DateTime date)
        {
            var newReport = new Report
            {
                AppointmentId = appointmentId,
                DoctorId = doctorId,
                VisitDate = date
            };
            return View(newReport);
        }


        // POST: Reports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReportId,AppointmentId,DoctorId,VisitDate,Symptoms,Diagnosis,Prescriptions")] Report report)
        {
            if (ModelState.IsValid)
            {
                _context.Add(report);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppointmentId"] = new SelectList(_context.Appointment, "AppointmentId", "AppointmentId", report.AppointmentId);
            ViewData["DoctorId"] = new SelectList(_context.Users, "Id", "Id", report.DoctorId);
            return View(report);
        }

        // GET: Reports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Report == null)
            {
                return NotFound();
            }

            var report = await _context.Report.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }
            ViewData["AppointmentId"] = new SelectList(_context.Appointment, "AppointmentId", "AppointmentId", report.AppointmentId);
            ViewData["DoctorId"] = new SelectList(_context.Users, "Id", "Name", report.DoctorId);
            return View(report);
        }

        // POST: Reports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReportId,AppointmentId,DoctorId,VisitDate,Symptoms,Diagnosis,Prescriptions")] Report report)
        {
            if (id != report.ReportId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(report);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReportExists(report.ReportId))
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
            ViewData["AppointmentId"] = new SelectList(_context.Appointment, "AppointmentId", "AppointmentId", report.AppointmentId);
            ViewData["DoctorId"] = new SelectList(_context.Users, "Id", "Id", report.DoctorId);
            return View(report);
        }

        // GET: Reports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Report == null)
            {
                return NotFound();
            }

            var report = await _context.Report
                .Include(r => r.Appointment)
                .Include(r => r.Doctor)
                .FirstOrDefaultAsync(m => m.ReportId == id);
            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        // POST: Reports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Report == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Report'  is null.");
            }
            var report = await _context.Report.FindAsync(id);
            if (report != null)
            {
                _context.Report.Remove(report);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReportExists(int id)
        {
          return (_context.Report?.Any(e => e.ReportId == id)).GetValueOrDefault();
        }
        public async Task<ActionResult> Print(int? id)
        {
            // Fetch the report data and pass it to the printable view
            var report = await  _context.Report
               .Include(r => r.Appointment)
               .Include(r => r.Doctor)
               .FirstOrDefaultAsync(m => m.ReportId == id);
            var patient = _context.Patient.FirstOrDefault(p => p.Id == report.Appointment.PatientId);
            ViewBag.PatientName = patient.Name;
            
            return View("ReportPrint", report);
        }
    }
}
