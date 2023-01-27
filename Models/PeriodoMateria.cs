using System.ComponentModel.DataAnnotations;

namespace GradeHoraria.Models
{
    public class PeriodoMateria
    {
        [Key]
        public int? Periodo_Id { get; set; }
        public virtual Periodo? Periodo { get; set; }

        [Key]
        public int? Materia_Id { get; set; }
        public virtual Materia? Materia { get; set; }
        public virtual ICollection<Periodo>? Periodos { get; set; }
        public virtual ICollection<Materia>? Materias { get; set; }
    }
}