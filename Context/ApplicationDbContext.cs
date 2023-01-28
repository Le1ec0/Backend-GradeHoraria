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

            modelBuilder.Entity<Periodo>()
                .HasOne(p => p.Curso)
                .WithMany(c => c.Periodos)
                .HasForeignKey(p => p.CursoId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Materia>()
                .HasOne(m => m.Periodo)
                .WithMany(c => c.Materias)
                .HasForeignKey(m => m.PeriodoId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Materia>()
                .HasOne(m => m.Curso)
                .WithMany(c => c.Materias)
                .HasForeignKey(m => m.CursoId)
                .OnDelete(DeleteBehavior.NoAction);
        }
        public override int SaveChanges()
        {
            var addedCursoEntities = ChangeTracker.Entries<Curso>()
                .Where(e => e.State == EntityState.Added)
                .Select(e => e.Entity);

            foreach (var addedCurso in addedCursoEntities)
            {
                var periodo = new Periodo
                {
                    CursoId = addedCurso.Id,
                    Curso = addedCurso,
                    Id = addedCurso.Periodo
                };
                Periodos.Add(periodo);
            }

            return base.SaveChanges();
        }
    }
}