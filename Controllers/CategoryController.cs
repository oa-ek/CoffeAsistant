using CafeAssistiant.Data;
using CafeAssistiant.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CafeAssistiant.Controllers
{
    [Authorize(Roles = "Admin")] // Приклад для адміна 
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.ToListAsync();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryName")] Category category, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
            {
                // Якщо є помилки валідації, повертаємо форму із повідомленнями
                return View(category);
            }

            // Обробка зображення, якщо завантажили
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "categories");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                category.ImagePath = $"/images/categories/{fileName}";
            }
            else
            {
                // Якщо не завантажили — лишаємо ImagePath null
                category.ImagePath = null;
            }

            try
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Логування, якщо потрібно: напр. _logger.LogError(ex, "Error creating category");
                ModelState.AddModelError("", "Не вдалося зберегти категорію: " + ex.Message);
                return View(category);
            }
        }




        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category category, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                // Перевірка, чи вибрано нове зображення
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Створення шляху до папки зображень
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/categories");

                    // Створення папки, якщо вона не існує
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    // Унікальне ім’я файлу
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);

                    // Повний шлях до файлу
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    // Копіювання зображення у файл
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    // Оновлення шляху до зображення
                    category.ImagePath = "/images/categories/" + fileName;
                }

                _context.Update(category);
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Помилка при оновленні категорії.");
                }
            }

            return View(category);
        }


        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            if (_context.Products.Any(p => p.CategoryId == id))
            {
                ModelState.AddModelError("", "Неможливо видалити категорію, у якій є продукти.");
                return View("Index", await _context.Categories.ToListAsync());
            }

            _context.Categories.Remove(category);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Помилка при видаленні категорії.");
                return View("Index", await _context.Categories.ToListAsync());
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
