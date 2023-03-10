using Microsoft.EntityFrameworkCore;
using GradeHoraria.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace GradeHoraria.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<IdentityRole> IdentityRole { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Periodo> Periodos { get; set; }
        public DbSet<Materia> Materias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserLogin<string>>()
            .HasKey(x => x.UserId);

            modelBuilder.Entity<IdentityUserRole<string>>()
            .HasKey(r => new { r.UserId, r.RoleId });

            modelBuilder.Entity<IdentityUserToken<string>>()
            .HasKey(x => x.UserId);

            modelBuilder.Entity<Curso>()
            .HasMany(c => c.Periodos)
            .WithOne(p => p.Cursos)
            .HasForeignKey(p => p.CursoId)
            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Periodo>()
            .HasMany(p => p.Materias)
            .WithOne(m => m.Periodos)
            .HasForeignKey(m => m.PeriodoId)
            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Periodo>()
            .HasOne(p => p.Cursos)
            .WithMany(c => c.Periodos)
            .HasForeignKey(p => p.CursoId)
            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Materia>()
            .HasOne(m => m.Periodos)
            .WithMany(c => c.Materias)
            .HasForeignKey(m => m.PeriodoId)
            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Materia>()
            .HasOne(m => m.Cursos)
            .WithMany(c => c.Materias)
            .HasForeignKey(m => m.CursoId)
            .OnDelete(DeleteBehavior.NoAction);
        }
    }
}