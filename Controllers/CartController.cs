using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private const string CartSessionKey = "Cart";
        private const string CartCookieKey = "CtrlSaveCart";
        private readonly Ctrl_SaveContext _context;

        public CartController(Ctrl_SaveContext context)
        {
            _context = context;
        }

        private CookieOptions GetCookieOptions() => new()
        {
            Expires = DateTimeOffset.Now.AddDays(7),
            HttpOnly = true,
            IsEssential = true,
            SameSite = SameSiteMode.Lax
        };

        private List<CartItem> GetCart()
        {
            var sessionJson = HttpContext.Session.GetString(CartSessionKey);
            if (!string.IsNullOrEmpty(sessionJson))
                return JsonSerializer.Deserialize<List<CartItem>>(sessionJson) ?? new();

            var cookieJson = Request.Cookies[CartCookieKey];
            if (!string.IsNullOrEmpty(cookieJson))
            {
                var cart = JsonSerializer.Deserialize<List<CartItem>>(cookieJson) ?? new();
                HttpContext.Session.SetString(CartSessionKey, cookieJson);
                return cart;
            }

            return new List<CartItem>();
        }

        private void SaveCart(List<CartItem> cart)
        {
            var json = JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString(CartSessionKey, json);
            Response.Cookies.Append(CartCookieKey, json, GetCookieOptions());
        }

        private void ClearCart()
        {
            HttpContext.Session.Remove(CartSessionKey);
            Response.Cookies.Delete(CartCookieKey);
        }

        public IActionResult Index() => View(GetCart());

        [HttpPost]
        public IActionResult Add(string id, string name, string price, string image, string category)
        {
            var cart = GetCart();
            if (!cart.Any(i => i.Id == id))
                cart.Add(new CartItem { Id = id, Name = name, Price = price, Image = image, Category = category });
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

        [HttpPost]
        public IActionResult UpdateQty(string id, int qty)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(i => i.Id == id);
            if (item != null) { item.Quantity = qty < 1 ? 1 : qty; SaveCart(cart); }
            return Json(new { success = true, count = cart.Sum(i => i.Quantity) });
        }

        [HttpGet]
        public IActionResult Contains(string id) => Json(new { inCart = GetCart().Any(i => i.Id == id) });

        [HttpGet]
        public IActionResult Count() => Json(new { count = GetCart().Sum(i => i.Quantity) });

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
            var orderTotal = cart.Sum(i =>
            {
                var priceStr = i.Price.Replace("N$", "").Replace(",", "");
                return decimal.TryParse(priceStr, out var p) ? p * i.Quantity : 0;
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
