using Microsoft.AspNetCore.Identity;

namespace GradeHoraria.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int? DiaDisponivel { get; set; }
        public virtual ICollection<Cursos>? Cursos { get; set; }
        public virtual ICollection<Materias>? Materias { get; set; }
    }
}