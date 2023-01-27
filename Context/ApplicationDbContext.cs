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
            .HasKey(cp => new { cp.CursoId, cp.PeriodoId });

            modelBuilder.Entity<CursoPeriodo>()
            .HasOne(cp => cp.Cursos)
            .WithMany(c => c.CursoPeriodos)
            .HasForeignKey(cp => cp.CursoId);

            modelBuilder.Entity<CursoPeriodo>()
            .HasOne(cp => cp.Periodos)
            .WithMany(p => p.CursoPeriodos)
            .HasForeignKey(cp => cp.PeriodoId);

            modelBuilder.Entity<PeriodoMateria>()
            .HasKey(pm => new { pm.MateriaId, pm.PeriodoId });

            modelBuilder.Entity<PeriodoMateria>()
            .HasOne(pm => pm.Materias)
            .WithMany(m => m.PeriodoMaterias)
            .HasForeignKey(pm => pm.MateriaId);

            modelBuilder.Entity<PeriodoMateria>()
            .HasOne(pm => pm.Periodos)
            .WithMany(p => p.PeriodoMaterias)
            .HasForeignKey(pm => pm.PeriodoId);
        }
    }
}