using Microsoft.EntityFrameworkCore;
using Backend_CarStore.Models;

namespace Backend_CarStore.Context
{
    public class UsersDbContext : DbContext
    {
        public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options)
        {

        }

        public DbSet<Users> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var users = modelBuilder.Entity<Users>();
            modelBuilder.Entity<Users>().ToTable("Users");
            users.HasKey(x => x.Id);
            users.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            users.Property(x => x.Name).HasColumnName("name").IsRequired();
            users.Property(x => x.Password).HasColumnName("password").IsRequired();
            users.Property(x => x.Email).HasColumnName("email").IsRequired();
            users.Property(x => x.Phone).HasColumnName("phone").IsRequired();
        }
    }
}