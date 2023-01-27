using System.ComponentModel.DataAnnotations;

namespace GradeHoraria.Models
{
    public class Periodo
    {
        [Key]
        public int Id { get; set; }
        public virtual ICollection<CursoPeriodo> CursoPeriodos { get; set; }
        public virtual ICollection<PeriodoMateria> PeriodoMaterias { get; set; }
    }
}