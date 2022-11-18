using Microsoft.EntityFrameworkCore;
using CarStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CarStore.Context
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Cars> Car { get; set; }
        //public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var cars = modelBuilder.Entity<Cars>();

            modelBuilder.Entity<Cars>().ToTable("Cars");
            cars.HasKey(x => x.CarId);
            cars.Property(x => x.CarId).HasColumnName("CarId").ValueGeneratedOnAdd();
            cars.Property(x => x.Plate).HasColumnName("Plate").IsRequired();
            cars.Property(x => x.Brand).HasColumnName("Brand").IsRequired();
            cars.Property(x => x.Model).HasColumnName("Model").IsRequired();
            cars.Property(x => x.Color).HasColumnName("Color").IsRequired();
            cars.Property(x => x.Year).HasColumnName("Year").IsRequired();
            cars.Property(x => x.Description).HasColumnName("Description").IsRequired();

            var users = modelBuilder.Entity<ApplicationUser>();
            modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers");
            users.HasKey(x => x.Id);
            users.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            users.Property(x => x.UserName).HasColumnName("username").IsRequired();
            users.Property(x => x.Password).HasColumnName("password").IsRequired();
            users.Property(x => x.Email).HasColumnName("email").IsRequired();
            users.Property(x => x.Phone).HasColumnName("phone").IsRequired();

            modelBuilder.Entity<ApplicationUser>()
            .HasOne(p => p.Cars)
            .WithMany(b => b.ApplicationUsers)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired()
            .HasForeignKey(p => p.CarId);

            /*var users = modelBuilder.Entity<Users>();
            modelBuilder.Entity<Users>().ToTable("Users");
            users.HasKey(x => x.Id);
            users.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            users.Property(x => x.UserName).HasColumnName("name").IsRequired();
            users.Property(x => x.Password).HasColumnName("password").IsRequired();
            users.Property(x => x.Email).HasColumnName("email").IsRequired();
            users.Property(x => x.Phone).HasColumnName("phone").IsRequired();*/

            base.OnModelCreating(modelBuilder);
        }
    }
}