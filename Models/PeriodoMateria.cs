using System.ComponentModel.DataAnnotations;

namespace GradeHoraria.Models
{
    public class PeriodoMateria
    {
        public ICollection<int>? Periodo_Id { get; set; }
        public Periodo? Periodo { get; set; }

        public int? Materia_Id { get; set; }
        public Materia? Materia { get; set; }
    }
}