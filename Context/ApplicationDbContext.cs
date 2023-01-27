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
        public DbSet<CursoPeriodo> CursoPeriodos { get; set; }
        public DbSet<PeriodoMateria> PeriodoMaterias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CursoPeriodo>()
                .HasKey(cp => new { cp.Curso_Id, cp.Periodo_Id });

            modelBuilder.Entity<CursoPeriodo>()
                .HasOne(cp => cp.Curso)
                .WithMany(c => c.CursoPeriodos)
                .HasForeignKey(cp => cp.Curso_Id);

            modelBuilder.Entity<CursoPeriodo>()
                .HasOne(cp => cp.Periodo)
                .WithMany(p => p.CursoPeriodos)
                .HasForeignKey(cp => cp.Periodo_Id);

            modelBuilder.Entity<PeriodoMateria>()
            .HasKey(pm => new { pm.Materia_Id, pm.Periodo_Id });

            modelBuilder.Entity<PeriodoMateria>()
            .HasOne(pm => pm.Materia)
                .WithMany(m => m.PeriodoMaterias)
                .HasForeignKey(pm => pm.Materia_Id);

            modelBuilder.Entity<PeriodoMateria>()
                .HasOne(pm => pm.Periodo)
                .WithMany(p => p.PeriodoMaterias)
                .HasForeignKey(pm => pm.Periodo_Id);

            modelBuilder.Entity<Materia>()
                .HasOne(m => m.CursoPeriodo)
                .WithMany(cp => cp.Materias)
                .HasForeignKey(m => new { m.Cursos_Id, m.Periodo_Id });
        }
    }
}