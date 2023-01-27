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
            modelBuilder.Entity<CursoPeriodo>()
            .HasKey(bc => new { bc.Curso_Id, bc.Periodo_Id, bc.Materia_Id });

            modelBuilder.Entity<CursoPeriodo>()
                .HasOne(bc => bc.Curso)
                .WithMany(b => b.CursoPeriodos)
                .HasForeignKey(bc => bc.Curso_Id);

            modelBuilder.Entity<CursoPeriodo>()
                .HasOne(bc => bc.Periodo)
                .WithMany(c => c.CursoPeriodos)
                .HasForeignKey(bc => bc.Periodo_Id);

            modelBuilder.Entity<CursoPeriodo>()
                .HasOne(bc => bc.Materia)
                .WithMany(c => c.CursoPeriodos)
                .HasForeignKey(bc => bc.Materia_Id);
        }

    }
}