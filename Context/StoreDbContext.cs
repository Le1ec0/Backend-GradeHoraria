using Microsoft.EntityFrameworkCore;
using Backend_CarStore.Models;

namespace Backend_CarStore.Context
{
    public class StoreDbContext : DbContext
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options)
        {

        }

        public DbSet<Cars> Car { get; set; }
        public DbSet<RegisterModel> User { get; set; }

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

            var users = modelBuilder.Entity<RegisterModel>();
            modelBuilder.Entity<RegisterModel>().ToTable("Users");
            users.HasKey(x => x.Id);
            users.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            users.Property(x => x.UserName).HasColumnName("name").IsRequired();
            users.Property(x => x.Password).HasColumnName("password").IsRequired();
            users.Property(x => x.Email).HasColumnName("email").IsRequired();
            users.Property(x => x.Phone).HasColumnName("phone").IsRequired();
        }
    }
}