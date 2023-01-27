using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GradeHoraria.Models
{
    public class Periodo
    {
        [Key]
        public int? Id { get; set; }
        public int? Curso_Id { get; set; }
        public Curso? Cursos { get; set; }
        public virtual ICollection<Materia>? Materias { get; set; }
        public virtual ICollection<CursoPeriodo>? CursoPeriodos { get; set; }
        public virtual ICollection<PeriodoMateria>? PeriodoMaterias { get; set; }
    }
}