using Microsoft.EntityFrameworkCore;
using Ctrl_Save.Models;

namespace Ctrl_Save.Models
{
    public class Ctrl_SaveContext : DbContext
    {
        public Ctrl_SaveContext(DbContextOptions<Ctrl_SaveContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<ErrorLog> ErrorLogs { get; set; }
    }
}