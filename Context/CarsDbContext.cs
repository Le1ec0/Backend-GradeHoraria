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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var cars = modelBuilder.Entity<Cars>();
            modelBuilder.Entity<Cars>().ToTable("Cars");
            cars.HasKey(x => x.Id);
            cars.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            cars.Property(x => x.Plate).HasColumnName("plate").IsRequired();
            cars.Property(x => x.Brand).HasColumnName("brand").IsRequired();
            cars.Property(x => x.Model).HasColumnName("model").IsRequired();
            cars.Property(x => x.Color).HasColumnName("color").IsRequired();
            cars.Property(x => x.Year).HasColumnName("year").IsRequired();
        }
    }
}