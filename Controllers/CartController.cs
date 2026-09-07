using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ctrl_Save.Models;
using System.Text.Json;

namespace Ctrl_Save.Controllers
{
    public class CartItem
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Price { get; set; } = "";
        public string Image { get; set; } = "";
        public string Category { get; set; } = "";
        public int Quantity { get; set; } = 1;
    }

    public class CheckoutViewModel
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Address { get; set; } = "";
        public string City { get; set; } = "";
        public string Region { get; set; } = "";
        public string PaymentMethod { get; set; } = "";
        public List<CartItem> Items { get; set; } = new();
        public string OrderNumber { get; set; } = "";
        public string OrderDate { get; set; } = "";
        public decimal OrderTotal { get; set; }
    }

    public class CartController : Controller
    {
        private readonly Ctrl_SaveContext _context;

        public CartController(Ctrl_SaveContext context)
        {
            _context = context;
        }

        private string CartKey => User.Identity?.IsAuthenticated == true
            ? $"Cart_{User.Identity.Name}"
            : "Cart_guest";

        private List<CartItem> GetCart()
        {
            var sessionJson = HttpContext.Session.GetString(CartKey);
            return string.IsNullOrEmpty(sessionJson) ? new List<CartItem>() : JsonSerializer.Deserialize<List<CartItem>>(sessionJson) ?? new();
        }

        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.SetString(CartKey, JsonSerializer.Serialize(cart));
        }

        private void ClearCart()
        {
            HttpContext.Session.Remove(CartKey);
        }

        public IActionResult Index() => View(GetCart());

        [HttpPost]
        public async Task<IActionResult> Add(string id, string name, string price, string image, string category)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
            if (product == null || !product.IsAvailable)
                return Json(new { success = false, message = "Sorry, this item is no longer available." });

            var cart = GetCart();
            if (!cart.Any(i => i.Id == id))
                cart.Add(new CartItem { Id = id, Name = name, Price = price, Image = image, Category = category, Quantity = 1 });
            SaveCart(cart);
            return Json(new { success = true, count = cart.Sum(i => i.Quantity) });
        }

        [HttpPost]
        public IActionResult Remove(string id)
        {
            var cart = GetCart();
            cart.RemoveAll(i => i.Id == id);
            SaveCart(cart);
            return Json(new { success = true, count = cart.Sum(i => i.Quantity) });
        }

        [HttpGet]
        public IActionResult Contains(string id) => Json(new { inCart = GetCart().Any(i => i.Id == id) });

        [HttpGet]
        public IActionResult Count()
        {
            return Json(new { count = GetCart().Sum(i => i.Quantity) });
        }

        [Authorize]
        public IActionResult Checkout()
        {
            var cart = GetCart();
            if (!cart.Any()) return RedirectToAction("Index");
            return View(new CheckoutViewModel { Items = cart });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PlaceOrder(CheckoutViewModel model)
        {
            var cart = GetCart();

            // Check all items are still available before placing order
            var soldItems = new List<string>();
            foreach (var item in cart)
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == item.Id);
                if (product == null || !product.IsAvailable)
                    soldItems.Add(item.Name);
            }

            if (soldItems.Any())
            {
                // Remove sold items from cart
                var soldIds = cart.Where(i => soldItems.Contains(i.Name)).Select(i => i.Id).ToList();
                cart.RemoveAll(i => soldIds.Contains(i.Id));
                SaveCart(cart);
                TempData["SoldItems"] = string.Join(", ", soldItems);
                return RedirectToAction("Index");
            }

            var orderTotal = cart.Sum(i =>
            {
                var priceStr = i.Price.Replace("N$", "").Replace(",", "");
                return decimal.TryParse(priceStr, out var p) ? p : 0;
            });

            var order = new Order
            {
                OrderNumber = "CS-" + DateTime.Now.ToString("yyyyMMdd") + "-" + new Random().Next(1000, 9999),
                FirstName = model.FirstName,
                LastName = model.LastName,
                Phone = model.Phone,
                Address = model.Address,
                City = model.City,
                Region = model.Region,
                PaymentMethod = model.PaymentMethod,
                OrderTotal = orderTotal,
                OrderDate = DateTime.Now,
                Items = cart.Select(i => new OrderItem
                {
                    ProductId = i.Id,
                    ProductName = i.Name,
                    Price = i.Price,
                    Category = i.Category
                }).ToList()
            };

            _context.Orders.Add(order);

            // Mark each product as sold immediately
            foreach (var item in cart)
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == item.Id);
                if (product != null)
                    product.IsAvailable = false;
            }

            await _context.SaveChangesAsync();
            ClearCart();

            model.Items = cart;
            model.OrderNumber = order.OrderNumber;
            model.OrderDate = order.OrderDate.ToString("dd MMM yyyy, HH:mm");
            model.OrderTotal = orderTotal;
            model.Region = order.Region;

            return View("OrderConfirmation", model);
        }
    }
}
