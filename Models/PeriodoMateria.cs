using System.ComponentModel.DataAnnotations.Schema;

namespace GradeHoraria.Models
{
    public class PeriodoMateria
    {
        [ForeignKey("Materia")]
        public int Materia_Id { get; set; }
        public Materia Materia { get; set; }

        [ForeignKey("Periodo")]
        public int Periodo_Id { get; set; }
        public Periodo Periodo { get; set; }
    }
}