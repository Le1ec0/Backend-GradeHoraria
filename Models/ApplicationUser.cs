using Microsoft.AspNetCore.Identity;

namespace GradeHoraria.Models
{
    public class ApplicationUser
    {
        public int? DiaDisponivel { get; set; }
        public string? DisplayName { get; set; }
        public string? Email { get; set; }
        public string? ObjectId { get; set; }
        public string? Upn { get; set; }
        public virtual ICollection<Curso>? Cursos { get; set; }
        public virtual ICollection<Materia>? Materias { get; set; }
    }
}