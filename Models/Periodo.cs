using System.ComponentModel.DataAnnotations;

namespace GradeHoraria.Models
{
    public class Periodo
    {
        [Key]
        public int Periodo_Id { get; set; }
        public int Cursos_Id { get; set; }
        public Curso? Cursos { get; set; }
        public virtual ICollection<Materia>? Materias { get; set; }
    }
}