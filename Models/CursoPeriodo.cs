using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GradeHoraria.Models
{
    public class CursoPeriodo
    {
        [Key]
        [ForeignKey("Curso")]
        public int Curso_Id { get; set; }
        public Curso Curso { get; set; }
        [Key]
        [ForeignKey("Periodo")]
        public int Periodo_Id { get; set; }
        public Periodo Periodo { get; set; }
    }
}