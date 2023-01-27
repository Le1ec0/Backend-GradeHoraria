using System.ComponentModel.DataAnnotations.Schema;

namespace GradeHoraria.Models
{
    public class CursoPeriodo
    {
        public int CursoId { get; set; }
        public int PeriodoId { get; set; }
    }
}