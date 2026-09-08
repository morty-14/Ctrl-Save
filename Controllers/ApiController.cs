using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using Ctrl_Save.Models;
using Ctrl_Save.Models.DTOs;
using Ctrl_Save.Repositories;

namespace Ctrl_Save.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting("fixed")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepo;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        public ProductsController(IProductRepository productRepo, IMapper mapper, IDistributedCache cache)
        {
            _productRepo = productRepo;
            _mapper = mapper;
            _cache = cache;
        }

        /// <summary>Get all products</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cacheKey = "all_products";
            var cached = await _cache.GetStringAsync(cacheKey);

            if (cached != null)
            {
                var cachedProducts = JsonSerializer.Deserialize<List<ProductDto>>(cached);
                return Ok(cachedProducts);
            }

            var products = await _productRepo.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<ProductDto>>(products);

            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(dtos),
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) });

            return Ok(dtos);
        }

        /// <summary>Get products by category</summary>
        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetByCategory(string category)
        {
            var cacheKey = $"products_category_{category.ToLower()}";
            var cached = await _cache.GetStringAsync(cacheKey);

            if (cached != null)
            {
                var cachedProducts = JsonSerializer.Deserialize<List<ProductDto>>(cached);
                return Ok(cachedProducts);
            }

            var products = await _productRepo.GetByCategoryAsync(category);
            var dtos = _mapper.Map<IEnumerable<ProductDto>>(products);

            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(dtos),
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) });

            return Ok(dtos);
        }

        /// <summary>Get a single product by ProductId</summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var cacheKey = $"product_{id}";
            var cached = await _cache.GetStringAsync(cacheKey);

            if (cached != null)
            {
                var cachedProduct = JsonSerializer.Deserialize<ProductDto>(cached);
                return Ok(cachedProduct);
            }

            var product = await _productRepo.GetByProductIdAsync(id);
            if (product == null) return NotFound();

            var dto = _mapper.Map<ProductDto>(product);
            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(dto),
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) });

            return Ok(dto);
        }

        /// <summary>Get available products only</summary>
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailable()
        {
            var cacheKey = "available_products";
            var cached = await _cache.GetStringAsync(cacheKey);

            if (cached != null)
            {
                var cachedProducts = JsonSerializer.Deserialize<List<ProductDto>>(cached);
                return Ok(cachedProducts);
            }

            var products = await _productRepo.GetAvailableAsync();
            var dtos = _mapper.Map<IEnumerable<ProductDto>>(products);

            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(dtos),
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) });

            return Ok(dtos);
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
        private readonly IOrderRepository _orderRepo;
        private readonly IMapper _mapper;

        public OrdersController(IOrderRepository orderRepo, IMapper mapper)
        {
            _orderRepo = orderRepo;
            _mapper = mapper;
        }

        /// <summary>Get recent orders</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderRepo.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<OrderDto>>(orders));
        }

        /// <summary>Get order by order number</summary>
        [HttpGet("{orderNumber}")]
        public async Task<IActionResult> GetByOrderNumber(string orderNumber)
        {
            var order = await _orderRepo.GetByOrderNumberAsync(orderNumber);
            if (order == null) return NotFound();
            return Ok(_mapper.Map<OrderDto>(order));
        }

        /// <summary>Get total revenue</summary>
        [HttpGet("revenue")]
        public async Task<IActionResult> GetRevenue()
        {
            var total = await _orderRepo.GetTotalRevenueAsync();
            return Ok(new { totalRevenue = total });
        }
    }
}
