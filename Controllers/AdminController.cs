using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ctrl_Save.Models;

namespace Ctrl_Save.Controllers
{
    public class AdminController : Controller
    {
        private readonly Ctrl_SaveContext _context;
        private readonly IWebHostEnvironment _env;

        public AdminController(Ctrl_SaveContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Auth");

            var orders = await _context.Orders
                .OrderByDescending(o => o.OrderDate)
                .Take(50)
                .Select(o => new Order
                {
                    Id = o.Id,
                    OrderNumber = o.OrderNumber,
                    FirstName = o.FirstName,
                    LastName = o.LastName,
                    Phone = o.Phone,
                    Address = o.Address,
                    City = o.City,
                    Region = o.Region,
                    PaymentMethod = o.PaymentMethod,
                    OrderTotal = o.OrderTotal,
                    OrderDate = o.OrderDate,
                    Items = o.Items
                })
                .ToListAsync();

            var products = await _context.Products
                .OrderBy(p => p.Category)
                .ThenBy(p => p.Name)
                .ToListAsync();

            var messages = await _context.ContactMessages
                .OrderByDescending(m => m.SentAt)
                .ToListAsync();

            ViewBag.Products = products;
            ViewBag.Messages = messages;
            ViewBag.TotalOrders = await _context.Orders.CountAsync();
            ViewBag.TotalRevenue = await _context.Orders.SumAsync(o => o.OrderTotal);
            ViewBag.TotalProducts = products.Count;
            ViewBag.UnreadMessages = messages.Count(m => !m.IsRead);

            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (order == null) return NotFound();
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> ReplyMessage(int id, string reply)
        {
            var message = await _context.ContactMessages.FindAsync(id);
            if (message == null) return NotFound();
            message.Reply = reply;
            message.RepliedAt = DateTime.Now;
            message.IsRead = true;
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> MarkMessageRead(int id)
        {
            var message = await _context.ContactMessages.FindAsync(id);
            if (message == null) return NotFound();
            message.IsRead = true;
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var message = await _context.ContactMessages.FindAsync(id);
            if (message == null) return NotFound();
            _context.ContactMessages.Remove(message);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file, string category)
        {
            if (file == null || file.Length == 0)
                return Json(new { success = false, message = "No file selected." });

            var allowed = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var ext = Path.GetExtension(file.FileName).ToLower();
            if (!allowed.Contains(ext))
                return Json(new { success = false, message = "Only JPG, PNG or WEBP files are allowed." });

            var folder = Path.Combine(_env.WebRootPath, "images", "products", category.ToLower());
            Directory.CreateDirectory(folder);

            var filename = Path.GetFileName(file.FileName);
            var filePath = Path.Combine(folder, filename);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return Json(new { success = true, filename });
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)
        {
            if (!User.Identity?.IsAuthenticated ?? true) return Unauthorized();
            product.IsAvailable = true;
            product.Listed = DateTime.Now.ToString("dd MMM yyyy");

            // Auto-generate ProductId from name and category
            var slug = product.Name.ToLower()
                .Replace(" ", "-")
                .Replace("'", "")
                .Replace(".", "");
            product.ProductId = $"{product.Category}-{slug}";

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return Json(new { success = true, newId = product.Id });
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(Product product)
        {
            if (!User.Identity?.IsAuthenticated ?? true) return Unauthorized();
            var existing = await _context.Products.FindAsync(product.Id);
            if (existing == null) return NotFound();
            existing.Name = product.Name;
            existing.Category = product.Category;
            existing.Price = product.Price;
            existing.Condition = product.Condition;
            existing.Description = product.Description;
            existing.Includes = product.Includes;
            existing.Image = product.Image;
            existing.IsAvailable = product.IsAvailable;
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (!User.Identity?.IsAuthenticated ?? true) return Unauthorized();
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }
    }
}
