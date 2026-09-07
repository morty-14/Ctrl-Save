using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ctrl_Save.Models;
using Ctrl_Save.Models.DTOs;

namespace Ctrl_Save.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly Ctrl_SaveContext _context;

        public ProductsController(Ctrl_SaveContext context)
        {
            _context = context;
        }

        /// <summary>Get all products</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _context.Products
                .Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Price = p.Price,
                    Category = p.Category,
                    Condition = p.Condition,
                    Image = p.Image,
                    Includes = p.Includes,
                    IsAvailable = p.IsAvailable,
                    Listed = p.Listed
                })
                .ToListAsync();
            return Ok(products);
        }

        /// <summary>Get products by category</summary>
        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetByCategory(string category)
        {
            var products = await _context.Products
                .Where(p => p.Category == category.ToLower())
                .Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Price = p.Price,
                    Category = p.Category,
                    Condition = p.Condition,
                    Image = p.Image,
                    Includes = p.Includes,
                    IsAvailable = p.IsAvailable,
                    Listed = p.Listed
                })
                .ToListAsync();
            return Ok(products);
        }

        /// <summary>Get a single product by ProductId</summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == id);
            if (product == null) return NotFound();
            return Ok(new ProductDto
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Price = product.Price,
                Category = product.Category,
                Condition = product.Condition,
                Image = product.Image,
                Includes = product.Includes,
                IsAvailable = product.IsAvailable,
                Listed = product.Listed
            });
        }

        /// <summary>Get available products only</summary>
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailable()
        {
            var products = await _context.Products
                .Where(p => p.IsAvailable)
                .Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Price = p.Price,
                    Category = p.Category,
                    Condition = p.Condition,
                    Image = p.Image,
                    Includes = p.Includes,
                    IsAvailable = p.IsAvailable,
                    Listed = p.Listed
                })
                .ToListAsync();
            return Ok(products);
        }

        /// <summary>Test error logging</summary>
        [HttpGet("trigger-test-error")]
        public IActionResult TestError()
        {
            throw new InvalidOperationException("Test error for Grafana dashboard demo");
        }

        /// <summary>Create a new product (demonstrates validation)</summary>
        [HttpPost]
        public IActionResult Create([FromBody] ProductCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(new { message = "Product is valid and would be created successfully." });
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly Ctrl_SaveContext _context;

        public OrdersController(Ctrl_SaveContext context)
        {
            _context = context;
        }

        /// <summary>Get recent orders</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _context.Orders
                .Include(o => o.Items)
                .OrderByDescending(o => o.OrderDate)
                .Take(50)
                .Select(o => new OrderDto
                {
                    OrderNumber = o.OrderNumber,
                    FirstName = o.FirstName,
                    LastName = o.LastName,
                    City = o.City,
                    Region = o.Region,
                    PaymentMethod = o.PaymentMethod,
                    OrderTotal = o.OrderTotal,
                    OrderDate = o.OrderDate,
                    Items = o.Items.Select(i => new OrderItemDto
                    {
                        ProductName = i.ProductName,
                        Price = i.Price,
                        Category = i.Category
                    }).ToList()
                })
                .ToListAsync();
            return Ok(orders);
        }

        /// <summary>Get order by order number</summary>
        [HttpGet("{orderNumber}")]
        public async Task<IActionResult> GetByOrderNumber(string orderNumber)
        {
            var order = await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);
            if (order == null) return NotFound();
            return Ok(new OrderDto
            {
                OrderNumber = order.OrderNumber,
                FirstName = order.FirstName,
                LastName = order.LastName,
                City = order.City,
                Region = order.Region,
                PaymentMethod = order.PaymentMethod,
                OrderTotal = order.OrderTotal,
                OrderDate = order.OrderDate,
                Items = order.Items.Select(i => new OrderItemDto
                {
                    ProductName = i.ProductName,
                    Price = i.Price,
                    Category = i.Category
                }).ToList()
            });
        }

        /// <summary>Get total revenue</summary>
        [HttpGet("revenue")]
        public async Task<IActionResult> GetRevenue()
        {
            var total = await _context.Orders.SumAsync(o => o.OrderTotal);
            return Ok(new { totalRevenue = total });
        }
    }
}
