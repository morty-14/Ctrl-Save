using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ctrl_Save.Models;

namespace Ctrl_Save.Controllers
{
    [Authorize]
    public class SellerController : Controller
    {
        private readonly Ctrl_SaveContext _context;
        private readonly IWebHostEnvironment _env;

        public SellerController(Ctrl_SaveContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // My Listings page
        public async Task<IActionResult> MyListings()
        {
            var sellerId = User.Identity?.Name;
            var products = await _context.Products
                .Where(p => p.SellerId == sellerId)
                .OrderByDescending(p => p.Id)
                .ToListAsync();
            return View(products);
        }

        // Add listing page
        public IActionResult AddListing()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddListing(Product product, IFormFile? imageFile)
        {
            var sellerId = User.Identity?.Name;
            var sellerName = User.Claims.FirstOrDefault(c => c.Type == "given_name")?.Value ?? sellerId;

            product.SellerId = sellerId;
            product.SellerName = sellerName;
            product.IsAvailable = true;
            product.Listed = DateTime.Now.ToString("dd MMM yyyy");
            product.Category = product.Category.ToLower();

            // Generate ProductId
            product.ProductId = product.Category + "-" + product.Name.ToLower()
                .Replace(" ", "-")
                .Replace("/", "-");

            // Handle image upload
            if (imageFile != null && imageFile.Length > 0)
            {
                var folder = Path.Combine(_env.WebRootPath, "images", "products", product.Category);
                Directory.CreateDirectory(folder);
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(folder, fileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await imageFile.CopyToAsync(stream);
                product.Image = fileName;
            }
            else
            {
                product.Image = "placeholder.jpg";
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteListing(int id)
        {
            var sellerId = User.Identity?.Name;
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id && p.SellerId == sellerId);
            if (product == null) return Json(new { success = false });
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }
    }
}
