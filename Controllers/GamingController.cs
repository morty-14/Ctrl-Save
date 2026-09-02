using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ctrl_Save.Models;

namespace Ctrl_Save.Controllers
{
    public class GamingController : Controller
    {
        private readonly Ctrl_SaveContext _context;

        public GamingController(Ctrl_SaveContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Where(p => p.Category == "gaming" && p.IsAvailable)
                .ToListAsync();
            return View(products);
        }

        public async Task<IActionResult> Detail(string id)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == id);
            if (product == null) return NotFound();
            return View(product);
        }
    }
}
