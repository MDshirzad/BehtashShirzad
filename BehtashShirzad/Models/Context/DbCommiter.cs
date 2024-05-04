using BehtashShirzad.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BehtashShirzad.Model.Context
{
    public class DbCommiter : DbContext
    {
        
         
        public DbCommiter() : base() { }

        public  DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
      public DbSet<ProductCategory> productCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var decimalProps = modelBuilder.Model
   .GetEntityTypes()
   .SelectMany(t => t.GetProperties())
   .Where(p => (System.Nullable.GetUnderlyingType(p.ClrType) ?? p.ClrType) == typeof(decimal));
            
            foreach (var property in decimalProps)
            {
                property.SetPrecision(10);
                property.SetScale(2);
            }

            modelBuilder.Entity<ProductCategory>()
    .HasKey(c => c.Id);

            modelBuilder.Entity<ProductCategory>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var str = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["MyConnection"];
            optionsBuilder.UseSqlServer(str);

            
        }

     

     

    }
}
