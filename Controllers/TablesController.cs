using Microsoft.AspNetCore.Mvc;
using CafeAssistiant.Models;
using System;
using System.Threading.Tasks;
using CafeAssistiant.Data;
using Microsoft.EntityFrameworkCore;

namespace CafeAssistiant.Controllers
{
    public class TablesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TablesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var tables = await _context.Tables.ToListAsync();
            return View(tables);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reserve(int tableId, string fromTime, string toTime)
        {
            var table = await _context.Tables.FindAsync(tableId);
            if (table != null && !table.IsReserved)
            {
                if (TimeSpan.TryParse(fromTime, out var from) && TimeSpan.TryParse(toTime, out var to))
                {
                    if (to > from) // Перевірка логіки часу
                    {
                        var today = DateTime.Today;
                        table.IsReserved = true;
                        table.ReservedFrom = today.Add(from);
                        table.ReservedUntil = today.Add(to);

                        await _context.SaveChangesAsync();
                    }
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int tableId)
        {
            var table = await _context.Tables.FindAsync(tableId);
            if (table != null && table.IsReserved)
            {
                table.IsReserved = false;
                table.ReservedFrom = null;
                table.ReservedUntil = null;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
