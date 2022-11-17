using Microsoft.EntityFrameworkCore;
using Backend_CarStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Backend_CarStore.Context
{
    public class StoreDbContext : IdentityDbContext<IdentityUser>
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options)
        {

        }

        public DbSet<Cars> Car { get; set; }
        //public DbSet<Users> User { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var cars = modelBuilder.Entity<Cars>();
            modelBuilder.Entity<Cars>().ToTable("Cars");
            cars.HasKey(x => x.Id);
            cars.Property(x => x.Id).HasColumnName("CarId").ValueGeneratedOnAdd();
            cars.Property(x => x.Plate).HasColumnName("Plate").IsRequired();
            cars.Property(x => x.Brand).HasColumnName("Brand").IsRequired();
            cars.Property(x => x.Model).HasColumnName("Model").IsRequired();
            cars.Property(x => x.Color).HasColumnName("Color").IsRequired();
            cars.Property(x => x.Year).HasColumnName("Year").IsRequired();
            cars.Property(x => x.Description).HasColumnName("Description").IsRequired();

            /*cars.HasOne(p => p.Users)
            .WithMany(b => b.Cars)
            .HasForeignKey(p => p.Id);*/

            /*var users = modelBuilder.Entity<Users>();
            modelBuilder.Entity<Users>().ToTable("Users");
            users.HasKey(x => x.Id);
            users.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            users.Property(x => x.Username).HasColumnName("name").IsRequired();
            users.Property(x => x.Password).HasColumnName("password").IsRequired();
            users.Property(x => x.Email).HasColumnName("email").IsRequired();
            users.Property(x => x.Phone).HasColumnName("phone").IsRequired();*/

            base.OnModelCreating(modelBuilder);
        }
    }
}