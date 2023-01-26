using System.ComponentModel.DataAnnotations;

namespace GradeHoraria.Models
{
    public class Periodo
    {
        [Key]
        public string? Semestre { get; set; }
        public int? CursoId { get; set; }
        public virtual Curso? Cursos { get; set; }
        public virtual ICollection<Materia>? Materias { get; set; }
    }
}