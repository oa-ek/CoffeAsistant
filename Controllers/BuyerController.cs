using CafeAssistiant.Data;
using CafeAssistiant.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CafeAssistiant.Controllers
{
    public class BuyerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BuyerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int categoryId = 0)
        {
            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "CategoryName");

            var products = _context.Products.Include(p => p.Category).AsQueryable();

            if (categoryId != 0)
            {
                products = products.Where(p => p.CategoryId == categoryId);
            }

            return View(await products.ToListAsync());
        }

    }
}
