using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GradeHoraria.Models
{
    public class CursoPeriodo
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Curso_Id")]
        public int Curso_Id { get; set; }
        public Curso Curso { get; set; }
        [ForeignKey("Periodo_Id")]
        public int Periodo_Id { get; set; }
        public Periodo Periodo { get; set; }
    }
}