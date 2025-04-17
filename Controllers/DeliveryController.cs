using CafeAssistiant.Data;
using CafeAssistiant.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CafeAssistiant.Controllers
{
    [Authorize]
    public class DeliveryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DeliveryController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var deliveries = await _context.Deliveries
                .Include(d => d.Order)
                .Include(d => d.ApplicationUser)
                .Include(d => d.Employee)
                .ToListAsync();

            return View(deliveries);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Orders = new SelectList(await _context.Orders.ToListAsync(), "Id", "Number");
            ViewBag.Customers = new SelectList(await _userManager.Users.ToListAsync(), "Id", "FullName");
            ViewBag.Employees = new SelectList(await _context.Employees.ToListAsync(), "Id", "EmployeeName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Delivery delivery)
        {
            if (ModelState.IsValid)
            {
                var order = await _context.Orders.FindAsync(delivery.OrderId);
                var customer = await _userManager.FindByIdAsync(delivery.ApplicationUserId);  // Замініть delivery.ApplicationUser
                var employee = await _context.Employees.FindAsync(delivery.EmployeeId);

                if (order == null || customer == null || employee == null)
                {
                    ModelState.AddModelError("", "Order, Customer або Employee не існує!");
                }
                else
                {
                    delivery.Ordered = DateTime.Now;
                    _context.Deliveries.Add(delivery);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.Orders = new SelectList(await _context.Orders.ToListAsync(), "Id", "Number");
            ViewBag.Customers = new SelectList(await _userManager.Users.ToListAsync(), "Id", "FullName");
            ViewBag.Employees = new SelectList(await _context.Employees.ToListAsync(), "Id", "EmployeeName");

            return View(delivery);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var delivery = await _context.Deliveries
                .Include(d => d.Order)
                .Include(d => d.ApplicationUser)
                .Include(d => d.Employee)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (delivery == null)
            {
                return NotFound();
            }

            ViewBag.Orders = new SelectList(await _context.Orders.ToListAsync(), "Id", "Number", delivery.OrderId);
            ViewBag.Customers = new SelectList(await _userManager.Users.ToListAsync(), "Id", "FullName", delivery.ApplicationUserId);
            ViewBag.Employees = new SelectList(await _context.Employees.ToListAsync(), "Id", "EmployeeName", delivery.EmployeeId);

            return View(delivery);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Delivery delivery)
        {
            if (id != delivery.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _context.Update(delivery);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Orders = new SelectList(await _context.Orders.ToListAsync(), "Id", "Number", delivery.OrderId);
            ViewBag.Customers = new SelectList(await _userManager.Users.ToListAsync(), "Id", "FullName", delivery.ApplicationUserId);
            ViewBag.Employees = new SelectList(await _context.Employees.ToListAsync(), "Id", "EmployeeName", delivery.EmployeeId);

            return View(delivery);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var delivery = await _context.Deliveries.FindAsync(id);
            if (delivery != null)
            {
                _context.Deliveries.Remove(delivery);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
