using System.ComponentModel.DataAnnotations;

namespace GradeHoraria.Models
{
    public class Periodo
    {
        [Key]
        public int Periodo_Id { get; set; }
        public ICollection<CursoPeriodo> CursoPeriodos { get; set; }
        public ICollection<PeriodoMateria> PeriodoMaterias { get; set; }
    }
}