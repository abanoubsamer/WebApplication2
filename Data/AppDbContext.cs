using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Data
{
    public class AppDbContext:DbContext
    {

        #region Constractor
        public AppDbContext()
        {

        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        #endregion

        #region Filde
        public DbSet<Product> Product { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<ProductImages> Images { get; set; }
        #endregion


        #region override

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=BIBOSAMER12;User ID=sa;Password=123456;Database=Product;TrustServerCertificate=True;");
            base.OnConfiguring(optionsBuilder);
        }

        #endregion

    }
}
