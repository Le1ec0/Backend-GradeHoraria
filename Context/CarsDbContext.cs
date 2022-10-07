using Microsoft.EntityFrameworkCore;
using Backend_CarStore.Models;

namespace Backend_CarStore.Context
{
    public class CarsDbContext : DbContext
    {
        public CarsDbContext(DbContextOptions<CarsDbContext> options) : base(options)
        {

        }

        public DbSet<Cars> Car { get; set; }
    }
}