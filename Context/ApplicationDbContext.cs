using Microsoft.EntityFrameworkCore;
using GradeHoraria.Models;

namespace GradeHoraria.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }
        public DbSet<Curso>? Cursos { get; set; }
        public DbSet<Periodo>? Periodos { get; set; }
        public DbSet<Materia>? Materias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Curso>()
                .HasMany(c => c.Periodos)
                .WithOne(p => p.Curso)
                .HasForeignKey(p => p.Curso_Id);

            modelBuilder.Entity<Periodo>()
                .HasMany(p => p.Materias)
                .WithOne(m => m.Periodo)
                .HasForeignKey(m => m.Periodo_Id);
        }

    }
}