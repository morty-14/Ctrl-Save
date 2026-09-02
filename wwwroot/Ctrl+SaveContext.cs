namespace Ctrl_Save.Models
using Microsoft.EntityFrameworkCore;
{
    public class Ctrl_SaveContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

    public Ctrl_SaveContext(DbContextOptions<Ctrl_SaveContext> options) : base(options)
    {

    }
    }
}
