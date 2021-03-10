using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContractorSearch.Data;
using ContractorSearch.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ContractorSearch.Controllers
{
    [Authorize(Roles = "Contractor")]
    public class ContractorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContractorController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var contractor = _context.Contractors.Where(c => c.IdentityUserId == userId).FirstOrDefault();
            if (contractor == null)
            {
                return RedirectToAction(nameof(Create));
            }
            else
            {
                return View(contractor);
            }
        }

        public IActionResult Chat()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,AddressLine1,AddressLine2,City,State,ZipCode,PhoneNumber, IdentityUserId")] Contractor contractor)
        {
            if (ModelState.IsValid)
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                contractor.IdentityUserId = userId;
                _context.Add(contractor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", contractor.IdentityUserId);
            return View(contractor);
        }

        public IActionResult CreateAppointments()
        {
            //ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAppointments(Appointment appointment)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var contr = _context.Contractors.Where(contr0 => contr0.IdentityUserId ==
            userId).FirstOrDefault();
            if (ModelState.IsValid)
            {
                appointment.ContractorId = contr.Id;
                appointment.CustomerId = 1; //need to leave this a variable
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", appointment.IdentityUserId);
            return View(appointment);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contractor = await _context.Contractors.FindAsync(id);
            if (contractor == null)
            {
                return NotFound();
            }
            ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", contractor.IdentityUserId);
            return View(contractor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,AddressLine1,AddressLine2,City,State,ZipCode,PhoneNumber,IdentityUserId")] Contractor contractor)
        {
            if (id != contractor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contractor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContractorExists(contractor.Id))
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
            ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", contractor.IdentityUserId);
            return View(contractor);
        }

        private bool ContractorExists(int id)
        {
            return _context.Contractors.Any(e => e.Id == id);
        }
    }
}
