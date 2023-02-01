using Microsoft.EntityFrameworkCore;
using GradeHoraria.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace GradeHoraria.Context
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<IdentityUser> IdentityUsers { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Periodo> Periodos { get; set; }
        public DbSet<Materia> Materias { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUser>()

            ;

            modelBuilder.Entity<IdentityUserLogin<string>>()
            .HasKey(x => new { x.LoginProvider, x.ProviderKey });
            //.HasNoKey()
            //;
            modelBuilder.Entity<IdentityUserRole<string>>()
            .HasKey(x => new { x.RoleId });
            //.HasNoKey()
            //;
            modelBuilder.Entity<IdentityUserToken<string>>()
            .HasKey(x => new { x.LoginProvider });
            //.HasNoKey()
            //;

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