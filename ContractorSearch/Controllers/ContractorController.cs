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
using ContractorSearch.Hubs;

namespace ContractorSearch.Controllers
{
    [Authorize(Roles = "Contractor")]
    public class ContractorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly TwilioService _twilioService;

        public ContractorController(ApplicationDbContext context, TwilioService twilioService)
        {
            _context = context;
            _twilioService = twilioService;
        }
        public async Task<IActionResult> Index()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var contractor = _context.Contractors.Where(c => c.IdentityUserId == userId).FirstOrDefault();
            if (contractor == null)
            {
                return RedirectToAction(nameof(Create));
            }
            else if (_context.Appointments.Count() == 0)
            {
                return RedirectToAction(nameof(CreateAppointments));
            }
            else
            {
                var applicationDbContext = _context.Appointments.Where(a => a.ContractorId == contractor.Id).ToListAsync();
                return View(await applicationDbContext);
            }
        }

        public IActionResult Chat()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var contractor = _context.Contractors.Where(c => c.IdentityUserId == userId).FirstOrDefault();

            return View(contractor);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contractor = await _context.Contractors
                .Include(c => c.IdentityUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contractor == null)
            {
                return NotFound();
            }

            return View(contractor);
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
                appointment.CustomerId = null;
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
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

        //public IActionResult SendConfirmationText()
        //{
        //    string messageToSend = "Your Appointment is Confirmed";
        //    _twilioService.SendText(messageToSend);
        //    return RedirectToAction(nameof(Index));
        //}
        //public IActionResult SendCustomText()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public IActionResult SendCustomText(string message)
        //{
        //    var messageToSend = Request.Form["messageToSend"];
        //    _twilioService.SendText(messageToSend);
        //    return RedirectToAction(nameof(Index));
        //}
    }
}
