using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ctrl_Save.Models;

namespace Ctrl_Save.Controllers
{
    public class SearchController : Controller
    {
        private readonly Ctrl_SaveContext _context;

        public SearchController(Ctrl_SaveContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string q)
        {
            ViewData["Query"] = q;
            if (string.IsNullOrWhiteSpace(q))
                return View(new List<Product>());

            var terms = q.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var results = await _context.Products
                .Where(p => p.IsAvailable && terms.All(term =>
                    p.Name.ToLower().Contains(term) ||
                    p.Category.ToLower().Contains(term) ||
                    p.ProductId.ToLower().Contains(term)))
                .ToListAsync();

            return View(results);
        }
    }
}
