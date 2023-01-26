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
                .HasMany(p => p.Periodos)
                .WithOne(c => c.Cursos)
                .HasForeignKey(i => i.Cursos_Id);

            modelBuilder.Entity<Periodo>()
                .HasMany(m => m.Materias)
                .WithOne(p => p.Periodos)
                .HasForeignKey(i => i.Semestre);

        }
    }
}