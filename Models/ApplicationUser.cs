using Microsoft.AspNetCore.Identity;

namespace GradeHoraria.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int? UserId { get; set; }
        public virtual ICollection<Cursos>? Cursos { get; set; }
        public virtual ICollection<Materias>? Materias { get; set; }
    }
}