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
            /*var cars = modelBuilder.Entity<Cars>();
            modelBuilder.Entity<Cars>().ToTable("Cars");
            cars.HasKey(x => x.CarId);
            cars.Property(x => x.CarId).HasColumnName("CarId").ValueGeneratedOnAdd();
            cars.Property(x => x.Plate).HasColumnName("Plate");
            cars.Property(x => x.Brand).HasColumnName("Brand");
            cars.Property(x => x.Model).HasColumnName("Model");
            cars.Property(x => x.Color).HasColumnName("Color");
            cars.Property(x => x.Year).HasColumnName("Year");
            cars.Property(x => x.Description).HasColumnName("Description");
            cars.Property(x => x.UserId).HasColumnName("UserId");*/

            /*var users = modelBuilder.Entity<ApplicationUser>();
            modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers");
            users.Property(x => x.CarId).HasColumnName("CarId");*/

            /*modelBuilder.Entity<Cars>()
            .HasOne(uc => uc.ApplicationUser)
            .WithMany(c => c.Cars)
            //.HasForeignKey(uc => uc.ApplicationUser)
            .OnDelete(DeleteBehavior.Cascade);
            //.IsRequired()*/

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