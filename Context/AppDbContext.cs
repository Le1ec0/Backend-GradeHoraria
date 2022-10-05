using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Backend_CarStore.Models;
using System.IO;

namespace Backend_CarStore.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .Build();

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("ServerConnection"));
        }
    }
}