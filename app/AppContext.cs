using Microsoft.EntityFrameworkCore;

namespace Company.Function
{
    public class AppContext : DbContext
    {
        public AppContext(DbContextOptions<AppContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Department> Departments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasAlternateKey(x => x.Name);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasIndex(x => x.DepartmentId);
                entity.HasOne(x => x.Department).WithMany(x => x.Products);
            });
        }
    }
}