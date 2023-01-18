using Microsoft.EntityFrameworkCore;
using GradeHoraria.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace GradeHoraria.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Cursos>? Cursos { get; set; }
        public DbSet<Materias>? Materias { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var cursos = modelBuilder.Entity<Cursos>();
            modelBuilder.Entity<Cursos>().ToTable("Cursos");
            cursos.HasKey(x => x.CursoId);

            var materias = modelBuilder.Entity<Materias>();
            modelBuilder.Entity<Materias>().ToTable("Materias");
            materias.HasKey(x => x.MateriaId);

            modelBuilder.Entity<ApplicationUser>()
            .HasOne<Cursos>(c => c.Cursos)
            .WithMany(u => u.ApplicationUsers)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}