using Microsoft.EntityFrameworkCore;
using GradeHoraria.Models;

namespace GradeHoraria.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Materia> Materias { get; set; }
        public DbSet<Periodo> Periodos { get; set; }
        public DbSet<CursoPeriodo> CursoPeriodos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Curso>()
                .HasMany(c => c.Periodos)
                .WithOne(p => p.Cursos)
                .HasForeignKey(p => p.Cursos_Id);

            modelBuilder.Entity<Periodo>()
                .HasMany(p => p.Materias)
                .WithOne(m => m.Periodos)
                .HasForeignKey(m => m.Periodo_Id);

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
        }

    }
}