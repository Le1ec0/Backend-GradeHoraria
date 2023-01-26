using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GradeHoraria.Models
{
    public class Periodo
    {
        [Key]
        [ForeignKey("Periodos")]
        public int? Periodo_Id { get; set; }
        [ForeignKey("Cursos")]
        public int? Cursos_Id { get; set; }
        public Curso? Cursos { get; set; }
        public virtual ICollection<Materia>? Materias { get; set; }
    }
}