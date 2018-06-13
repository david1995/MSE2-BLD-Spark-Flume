using Microsoft.EntityFrameworkCore;

namespace SparkFlume.Output.Entities
{
    public class ProductDbContext : DbContext
    {
        private readonly string _dbaddress;
        private readonly string _database;

        public ProductDbContext(string dbaddress, string database)
        {
            _dbaddress = dbaddress;
            _database = database;
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseMySQL($@"Server={_dbaddress};Database={_database};Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>()
                        .HasKey(product => new { product.Id, product.Minute });
        }
    }
}
