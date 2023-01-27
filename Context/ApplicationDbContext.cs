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
        public DbSet<CursoPeriodo> CursoPeriodo { get; set; }
        public DbSet<PeriodoMateria> PeriodoMateria { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CursoPeriodo>()
            .HasKey(cp => new { cp.Cursos, cp.Periodos });

            modelBuilder.Entity<CursoPeriodo>()
            .HasOne(cp => cp.Cursos)
            .WithMany(c => c.CursoPeriodos)
            .HasForeignKey(cp => cp.Cursos);

            modelBuilder.Entity<CursoPeriodo>()
            .HasOne(cp => cp.Periodos)
            .WithMany(p => p.CursoPeriodos)
            .HasForeignKey(cp => cp.Periodos);

            modelBuilder.Entity<PeriodoMateria>()
            .HasKey(pm => new { pm.Materias, pm.Periodos });

            modelBuilder.Entity<PeriodoMateria>()
            .HasOne(pm => pm.Materias)
            .WithMany(m => m.PeriodoMaterias)
            .HasForeignKey(pm => pm.Materias);

            modelBuilder.Entity<PeriodoMateria>()
            .HasOne(pm => pm.Periodos)
            .WithMany(p => p.PeriodoMaterias)
            .HasForeignKey(pm => pm.Periodos);
        }
    }
}