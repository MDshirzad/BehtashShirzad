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
