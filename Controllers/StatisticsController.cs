using CafeAssistiant.Data;
using CafeAssistiant.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace CafeAssistiant.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatisticsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult MostPopularProducts(string period = "today")
        {
            DateTime startDate = period switch
            {
                "yesterday" => DateTime.Today.AddDays(-1),
                "last7days" => DateTime.Today.AddDays(-7),
                "all" => DateTime.MinValue,
                _ => DateTime.Today
            };

            var filteredOrders = _context.Orders
                .Where(o => o.DateOrder >= startDate)
                .ToList();

            var popularProducts = filteredOrders
                .GroupBy(o => o.ProductId)
                .Select(group => new PopularProductViewModel
                {
                    ProductName = _context.Products.FirstOrDefault(p => p.Id == group.Key)?.Name ?? "Unknown",
                    OrderCount = group.Count(),
                    TotalRevenue = group.Sum(o => o.Product != null ? o.Product.Price : 0),
                    LastOrderDate = group.Max(o => o.DateOrder)
                })
                .OrderByDescending(p => p.OrderCount)
                .Take(10)
                .ToList();

            ViewBag.TotalRevenue = popularProducts.Sum(p => p.TotalRevenue).ToString("C2", new System.Globalization.CultureInfo("uk-UA"));
            ViewBag.Period = period;

            return View(popularProducts);
        }

        [HttpPost]
        public IActionResult ExportCsv(string period = "today")
        {
            DateTime startDate = period switch
            {
                "yesterday" => DateTime.Today.AddDays(-1),
                "last7days" => DateTime.Today.AddDays(-7),
                "all" => DateTime.MinValue,
                _ => DateTime.Today
            };

            var filteredOrders = _context.Orders
                .Where(o => o.DateOrder >= startDate)
                .ToList();

            var popularProducts = filteredOrders
                .GroupBy(o => o.ProductId)
                .Select(group => new
                {
                    ProductName = _context.Products.FirstOrDefault(p => p.Id == group.Key)?.Name ?? "Unknown",
                    OrderCount = group.Count(),
                    TotalRevenue = group.Sum(o => o.Product != null ? o.Product.Price : 0),
                    LastOrderDate = group.Max(o => o.DateOrder)
                })
                .OrderByDescending(p => p.OrderCount)
                .ToList();

            var csv = new StringBuilder();
            csv.AppendLine("Продукт,Кількість замовлень,Загальний дохід,Останнє замовлення");

            foreach (var p in popularProducts)
            {
                var lastOrderDateStr = p.LastOrderDate.ToString("dd.MM.yyyy HH:mm");
                var totalRevenueStr = $"{p.TotalRevenue:0.00} ₴";
                csv.AppendLine($"\"{p.ProductName}\",{p.OrderCount},{totalRevenueStr},{lastOrderDateStr}");
            }

            var utf8Bom = Encoding.UTF8.GetPreamble();
            var csvBytes = Encoding.UTF8.GetBytes(csv.ToString());
            var bytesWithBom = new byte[utf8Bom.Length + csvBytes.Length];
            Buffer.BlockCopy(utf8Bom, 0, bytesWithBom, 0, utf8Bom.Length);
            Buffer.BlockCopy(csvBytes, 0, bytesWithBom, utf8Bom.Length, csvBytes.Length);

            return File(bytesWithBom, "text/csv", $"statistics_{period}_{DateTime.Now:yyyyMMdd}.csv");
        }
    }
}
