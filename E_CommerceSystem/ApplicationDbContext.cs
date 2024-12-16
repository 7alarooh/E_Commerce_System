using E_CommerceSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceSystem
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<OrderProducts> OrderProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderProducts>()
                .HasKey(op => new { op.OrderId, op.ProductId }); // Composite Key
        }
    }
}
