using System.ComponentModel.DataAnnotations.Schema;

namespace GradeHoraria.Models
{
    public class Periodo
    {
        public string Semestre { get; set; }
        public string Sala { get; set; }
        public int CursoId { get; set; }
        public ICollection<CursoPeriodo> CursoPeriodos { get; set; }
        public ICollection<PeriodoMateria> PeriodoMaterias { get; set; }
    }
}