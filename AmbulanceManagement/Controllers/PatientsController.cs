using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AmbulanceManagement.Data;
using AmbulanceManagement.Models;
using Microsoft.AspNetCore.Authorization;

namespace AmbulanceManagement.Controllers
{
    public class PatientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PatientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Patients
        [Authorize]
        public async Task<IActionResult> Index(string searchQuery)
        {
            IQueryable<Patient> patientsQuery = _context.Patient;

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                patientsQuery = patientsQuery.Where(p =>
                    p.Name.Contains(searchQuery) ||
                    p.LastName.Contains(searchQuery) ||
                    p.Adress.Contains(searchQuery) ||
                    p.PhoneNumber.Contains(searchQuery) ||
                    p.EmailAddress.Contains(searchQuery)
                );
            }

            var patients = await patientsQuery.ToListAsync();

            return View(patients);
        }


        // GET: Patients/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Patient == null)
            {
                return NotFound();
            }

           
            var patient = await _context.Patient
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            var reports = _context.Report.Where(r => r.Appointment.PatientId == patient.Id).ToList();

            ViewData["Reports"] = reports;

            return View(patient);
        }

        // GET: Patients/Create
        [Authorize(Roles = "Nurse,Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Nurse,Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,LastName,EmailAddress,PhoneNumber,Age,Adress")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                _context.Add(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(patient);
        }

        // GET: Patients/Edit/5
        [Authorize(Roles = "Nurse,Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Patient == null)
            {
                return NotFound();
            }

            var patient = await _context.Patient.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Nurse,Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,LastName,EmailAddress,PhoneNumber,Age,Adress")] Patient patient)
        {
            if (id != patient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(patient.Id))
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
            return View(patient);
        }

        // GET: Patients/Delete/5
        [Authorize(Roles = "Nurse,Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Patient == null)
            {
                return NotFound();
            }

            var patient = await _context.Patient
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Nurse,Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Patient == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Patient'  is null.");
            }
            var patient = await _context.Patient.FindAsync(id);
            if (patient != null)
            {
                _context.Patient.Remove(patient);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(int id)
        {
          return (_context.Patient?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
