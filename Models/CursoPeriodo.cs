using System.ComponentModel.DataAnnotations.Schema;

namespace GradeHoraria.Models
{
    public class CursoPeriodo
    {
        public Curso Cursos { get; set; }
        public Periodo Periodos { get; set; }
    }
}