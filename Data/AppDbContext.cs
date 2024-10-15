using Microsoft.EntityFrameworkCore;
using TestTask1.Models;

namespace TestTask1.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<FileLineModel> FileLine { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Database=TestTask1_B1;Username=postgres;Password=12345678;Server=localhost;Port=5432;");
        }

    }
}
