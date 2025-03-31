using Dotnet8WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Dotnet8WebAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
    }
}
