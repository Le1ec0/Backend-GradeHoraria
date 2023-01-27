using System.ComponentModel.DataAnnotations;

namespace GradeHoraria.Models
{
    public class CursoPeriodo
    {
        [Key]
        public int? Curso_Id { get; set; }
        public virtual Curso? Curso { get; set; }

        [Key]
        public int? Periodo_Id { get; set; }
        public virtual Periodo? Periodo { get; set; }
    }
}