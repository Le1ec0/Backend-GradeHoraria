using System.ComponentModel.DataAnnotations;

namespace GradeHoraria.Models
{
    public class CursoPeriodo
    {
        [Key]
        public int Id { get; set; }
        public int Curso_Id { get; set; }
        public Curso Curso { get; set; }
        public int Periodo_Id { get; set; }
        public Periodo Periodo { get; set; }
        public virtual ICollection<Materia> Materias { get; set; }
    }
}