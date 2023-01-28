using Microsoft.EntityFrameworkCore;
using GradeHoraria.Models;

namespace GradeHoraria.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Periodo> Periodos { get; set; }
        public DbSet<Materia> Materias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Curso>()
                .HasMany(c => c.Periodos)
                .WithOne(p => p.Curso)
                .HasForeignKey(p => p.CursoId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Periodo>()
                .HasMany(p => p.Materias)
                .WithOne(m => m.Periodo)
                .HasForeignKey(m => m.PeriodoId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Materia>()
                .HasOne(m => m.Curso)
                .WithMany(c => c.Materias)
                .HasForeignKey(m => m.CursoId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}