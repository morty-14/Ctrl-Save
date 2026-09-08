using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public ProductsController(IProductRepository productRepo, IMapper mapper)
        {
            _productRepo = productRepo;
            _mapper = mapper;
        }

        /// <summary>Get all products</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productRepo.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<ProductDto>>(products));
        }

        /// <summary>Get products by category</summary>
        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetByCategory(string category)
        {
            var products = await _productRepo.GetByCategoryAsync(category);
            return Ok(_mapper.Map<IEnumerable<ProductDto>>(products));
        }

        /// <summary>Get a single product by ProductId</summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var product = await _productRepo.GetByProductIdAsync(id);
            if (product == null) return NotFound();
            return Ok(_mapper.Map<ProductDto>(product));
        }

        /// <summary>Get available products only</summary>
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailable()
        {
            var products = await _productRepo.GetAvailableAsync();
            return Ok(_mapper.Map<IEnumerable<ProductDto>>(products));
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
