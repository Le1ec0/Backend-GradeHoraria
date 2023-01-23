using Microsoft.EntityFrameworkCore;
using GradeHoraria.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace GradeHoraria.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Cursos> Cursos { get; set; }
        public DbSet<Materias> Materias { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>()
            .HasMany(au => au.Cursos)
            .WithOne(c => c.ApplicationUser)
            .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<ApplicationUser>()
            .HasMany(au => au.Materias)
            .WithOne(c => c.ApplicationUser)
            .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<Cursos>()
            .HasMany(c => c.Materias)
            .WithOne(m => m.Cursos)
            .HasForeignKey(m => m.MateriaId)
            .IsRequired(false);

            modelBuilder.Entity<Materias>()
            .HasOne(c => c.Cursos)
            .WithMany(m => m.Materias)
            .HasForeignKey(m => m.CursoId)
            .IsRequired(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}