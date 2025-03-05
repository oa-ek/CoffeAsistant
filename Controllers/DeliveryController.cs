using CafeAssistiant.Data;
using CafeAssistiant.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CafeAssistiant.Controllers
{
    public class DeliveryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DeliveryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var deliveries = _context.Deliveries.Include(d => d.Order).Include(d => d.Customer).Include(d => d.Employee).ToList();
            return View(deliveries);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Orders = new SelectList(_context.Orders, "Id", "Number");
            ViewBag.Customers = new SelectList(_context.Customers, "Id", "CustomerName");
            ViewBag.Employees = new SelectList(_context.Employees, "Id", "EmployeeName");

            return View();
        }

        [HttpPost]
        public IActionResult Create(Delivery delivery)
        {
            if (ModelState.IsValid)
            {
                var order = _context.Orders.Find(delivery.OrderId);
                var customer = _context.Customers.Find(delivery.CustomerId);
                var employee = _context.Employees.Find(delivery.EmployeeId);

                if (order != null && customer != null && employee != null)
                {
                    delivery.Ordered = DateTime.Now;

                    _context.Deliveries.Add(delivery);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Order, Customer або Employee не існує!");
                }
            }

            ViewBag.Orders = new SelectList(_context.Orders, "Id", "Number");
            ViewBag.Customers = new SelectList(_context.Customers, "Id", "CustomerName");
            ViewBag.Employees = new SelectList(_context.Employees, "Id", "EmployeeName");

            return View(delivery);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var delivery = _context.Deliveries.Find(id);
            ViewBag.Orders = new SelectList(_context.Orders, "Id", "Number");
            ViewBag.Customers = new SelectList(_context.Customers, "Id", "CustomerName");
            ViewBag.Employees = new SelectList(_context.Employees, "Id", "EmployeeName");

            return View(delivery);
        }

        [HttpPost]
        public IActionResult Edit(int id, Delivery delivery)
        {
            if (ModelState.IsValid)
            {
                _context.Deliveries.Update(delivery);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Orders = new SelectList(_context.Orders, "Id", "Number");
            ViewBag.Customers = new SelectList(_context.Customers, "Id", "CustomerName");
            ViewBag.Employees = new SelectList(_context.Employees, "Id", "EmployeeName");
            return View(delivery);
        }

        public IActionResult Delete(int id)
        {
            var delivery = _context.Deliveries.Find(id);
            if (delivery != null)
            {
                _context.Deliveries.Remove(delivery);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
