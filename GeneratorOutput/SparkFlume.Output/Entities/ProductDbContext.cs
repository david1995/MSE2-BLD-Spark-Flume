using Microsoft.EntityFrameworkCore;

namespace SparkFlume.Output.Entities
{
    public class ProductDbContext : DbContext
    {
        private readonly string _dbaddress;
        private readonly string _database;
        private readonly string _username;
        private readonly string _password;

        public ProductDbContext()
        {
        }

        public ProductDbContext(string dbaddress, string database, string username, string password)
        {
            _dbaddress = dbaddress;
            _database = database;
            _username = username;
            _password = password;
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseMySql($"Server={_dbaddress};Database={_database}; Uid={_username}; Pwd={_password}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>()
                        .HasKey(product => new { product.Id, product.Minute });
        }
    }
}
