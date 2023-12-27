using Microsoft.EntityFrameworkCore;
using Point_Of_Sales.Entities;

namespace Point_Of_Sales.Config
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Purchase> PurchaseHistories { get; set; }
        public DbSet<PurchaseDetail> PurchaseDetails { get; set; }
        public DbSet<RetailStore> RetailStores { get; set; }


        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>()
                .HasIndex(p => new { p.Product_Name, p.Barcode })
                .IsUnique();

            builder.Entity<Account>()
                .HasOne(a => a.Employee)
                .WithOne(e => e.Account)
                .HasForeignKey<Account>(e => e.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
                
            //var account = new Account()
            //{
            //    Username = "admin",
            //    Pwd = "admin",
            //    Role = "Head",
            //};
            //var employee = new Employee()
            //{
            //    Account = account,
            //    Fullname = "head",
            //    Email = "head@gmail.com",
            //    RetailStore = new RetailStore() { Name = "Head 1" }
            //};

            //account.Employee = employee;
            //builder.Entity<Employee>().HasData(employee);
            //builder.Entity<Account>().HasData(account);
        }
    }
}
