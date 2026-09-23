using Ctrl_Save.Models;
using Microsoft.EntityFrameworkCore;

namespace Ctrl_Save.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly Ctrl_SaveContext _context;

        public OrderRepository(Ctrl_SaveContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllAsync(int take = 50)
            => await _context.Orders
                .Include(o => o.Items)
                .OrderByDescending(o => o.OrderDate)
                .Take(take)
                .ToListAsync();

        public async Task<Order?> GetByOrderNumberAsync(string orderNumber)
            => await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);

        public async Task<decimal> GetTotalRevenueAsync()
            => await _context.Orders.SumAsync(o => o.OrderTotal);

        public async Task AddAsync(Order order)
            => await _context.Orders.AddAsync(order);

        public Task DeleteAsync(Order order)
        {
            _context.Orders.Remove(order);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}
