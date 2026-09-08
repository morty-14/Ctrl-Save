using Ctrl_Save.Models;
using Microsoft.EntityFrameworkCore;

namespace Ctrl_Save.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly Ctrl_SaveContext _context;

        public ProductRepository(Ctrl_SaveContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
            => await _context.Products.ToListAsync();

        public async Task<IEnumerable<Product>> GetByCategoryAsync(string category)
            => await _context.Products
                .Where(p => p.Category == category.ToLower())
                .ToListAsync();

        public async Task<Product?> GetByProductIdAsync(string productId)
            => await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == productId);

        public async Task<IEnumerable<Product>> GetAvailableAsync()
            => await _context.Products
                .Where(p => p.IsAvailable)
                .ToListAsync();

        public async Task<Product?> GetByIdAsync(int id)
            => await _context.Products.FindAsync(id);

        public async Task AddAsync(Product product)
            => await _context.Products.AddAsync(product);

        public Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Product product)
        {
            _context.Products.Remove(product);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}
