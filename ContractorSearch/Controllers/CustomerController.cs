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
    [Authorize(Roles = "Customer")]
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customer = _context.Customers.Where(c => c.IdentityUserId == userId).FirstOrDefault();
            if (customer == null)
            {
                return RedirectToAction(nameof(Create));
            }
            else if(_context.Contractors.Count() == 0)
            {
                return RedirectToAction(nameof(UnavailablePage));
            }
             else
            {
                return RedirectToAction(nameof(AvailableContractorsIndex));
            }
 
        }
        public IActionResult UnavailablePage()
        {
            return View();
        }

        public IActionResult Chat()
        {
            return View();
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.IdentityUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,MiddleInitial,LastName,AddressLine1,AddressLine2,City,State,ZipCode,PhoneNumber,IdentityUserId")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                customer.IdentityUserId = userId;
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", customer.IdentityUserId);
            return View(customer);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", customer.IdentityUserId);
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,MiddleInitial,LastName,AddressLine1,AddressLine2,City,State,ZipCode,PhoneNumber,IdentityUserId")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
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
            ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", customer.IdentityUserId);
            return View(customer);
        }        

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }

        public async Task<IActionResult> AvailableContractorsIndex()
        {
            var applicationDbContext = _context.Contractors.ToListAsync();
            return View(await applicationDbContext);
        }

        public async Task<IActionResult> AvailableAppointments(int? id)
        {
            var applicationDbContext = _context.Appointments.Where(a => a.ContractorId == id).ToListAsync();
            return View(await applicationDbContext);
        }

        public async Task<IActionResult> Reserve(int id)
        {
            var appointment = await _context.Appointments.FirstOrDefaultAsync(m => m.Id == id);
            appointment.ReservedAppointment = true;
            appointment.Status = "Reserved";
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> CurrentAppointments()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customer = _context.Customers.Where(c => c.IdentityUserId == userId).FirstOrDefault();
            var applicationDbContext = _context.Appointments.Where(a => a.CustomerId == customer.Id).ToListAsync();
            return View(await applicationDbContext);
        }
    }
}
