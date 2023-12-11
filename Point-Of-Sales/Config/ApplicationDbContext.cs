using Microsoft.EntityFrameworkCore;
using Point_Of_Sales.Entities;

namespace Point_Of_Sales.Config
{
    public class ApplicationDbContext : DbContext
    { 
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Employee> Employees { get; set;}
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<PurchaseHistory> PurchaseHistories { get; set; }
        public DbSet<PurchaseDetail> PurchaseDetails { get; set; }
        public DbSet<RetailStore> RetailStores { get; set; }


        public ApplicationDbContext(DbContextOptions options ) : base(options) { }  

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>()
                .HasIndex(p => new {p.Product_Name, p.Barcode})
                .IsUnique();

        }
    }
}
