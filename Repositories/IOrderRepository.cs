using Ctrl_Save.Models;

namespace Ctrl_Save.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync(int take = 50);
        Task<Order?> GetByOrderNumberAsync(string orderNumber);
        Task<decimal> GetTotalRevenueAsync();
        Task AddAsync(Order order);
        Task DeleteAsync(Order order);
        Task SaveChangesAsync();
    }
}
