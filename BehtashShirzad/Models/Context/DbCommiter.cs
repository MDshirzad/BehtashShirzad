using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ElliotStore.Model.Context
{
    public class DbCommiter : DbContext
    {
        
         
        public DbCommiter() : base(GetOptions()) { }

        public  DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
      

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

            // Additional configuration as needed
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var str = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["MyConnection"];
            optionsBuilder.UseSqlServer(str);

            
        }

        private static DbContextOptions GetOptions()
        {

            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var connectionString = config["ConnectionStrings:DefaultConnection"];
            var options = new DbContextOptionsBuilder();
            options.UseSqlServer(connectionString);
            return options.Options;
        }

     

    }
}
