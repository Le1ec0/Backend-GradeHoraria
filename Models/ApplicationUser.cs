using Microsoft.AspNetCore.Identity;

namespace GradeHoraria.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int? UserId { get; set; }
        public int? CarId { get; set; }
        public Materias? Materias { get; set; }
        public Cursos? Cursos { get; set; }
    }
}