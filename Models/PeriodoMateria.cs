using System.ComponentModel.DataAnnotations.Schema;

namespace GradeHoraria.Models
{
    public class PeriodoMateria
    {
        public int MateriaId { get; set; }
        public Materia Materias { get; set; }
        public int PeriodoId { get; set; }
        public Periodo Periodos { get; set; }
    }
}