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
    [Authorize(Roles = "Customer")]
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly GoogleMapsService _googleMapsService;

        public CustomerController(ApplicationDbContext context, GoogleMapsService googleMapsService)
        {
            _context = context;
            _googleMapsService = googleMapsService;
        }

        public IActionResult Index()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customer = _context.Customers.Where(c => c.IdentityUserId == userId).FirstOrDefault();
            if (customer == null)
            {
                return RedirectToAction(nameof(Create));
            }
            else
            {
                return View(customer);
            }

        }
        public IActionResult UnavailablePage()
        {
            return View();
        }

        public IActionResult Chat(int? id)
        {
            var appointment = _context.Appointments.Where(a => a.Id == id).FirstOrDefault();
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customer = _context.Customers.Where(c => c.IdentityUserId == userId).FirstOrDefault();
            ViewBag.Customer = customer;
            return View(appointment);
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
                customer = await _googleMapsService.GeocodeCustomerAddress(customer);
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
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customer = _context.Customers.Where(c => c.IdentityUserId == userId).FirstOrDefault();
            var appointment = await _context.Appointments.FirstOrDefaultAsync(m => m.Id == id);
            appointment.ReservedAppointment = true;
            appointment.Status = "Reserved";
            appointment.CustomerId = customer.Id;
            appointment.Customer = customer;
            appointment.Contractor = appointment.Contractor;
            appointment.ContractorId = appointment.ContractorId;
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

        public async Task<IActionResult> SeeContractorReviewsAndRating(int contID)
        {
            var contId = contID;
            var reviews = _context.Appointments.Where(c => c.ContractorId == contId);
            return View();
        }

        public IActionResult RateAndReview(int id)
        {
            var apptDetails = _context.Appointments.Find(id);
            return View(apptDetails);
        }
        [HttpPost]
        public IActionResult RateAndReview(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}
