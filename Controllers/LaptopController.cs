using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ctrl_Save.Models;

namespace Ctrl_Save.Controllers
{
    public class LaptopController : Controller
    {
        private readonly Ctrl_SaveContext _context;
        private readonly ILogger<LaptopController> _logger;

        public LaptopController(Ctrl_SaveContext context, ILogger<LaptopController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Where(p => p.Category == "laptop" && p.IsAvailable)
                .ToListAsync();
            return View(products);
        }

        public async Task<IActionResult> Detail(string id)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == id);
            if (product == null) return NotFound();

            _logger.LogInformation("product_viewed product_name={ProductName} category={Category}",
                product.Name, product.Category);

            return View(product);
        }
    }
}