using System.ComponentModel.DataAnnotations.Schema;

namespace GradeHoraria.Models
{
    public class PeriodoMateria
    {
        public Materia Materias { get; set; }
        public Periodo Periodos { get; set; }
    }
}