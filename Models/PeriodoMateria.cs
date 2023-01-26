using System.ComponentModel.DataAnnotations;

namespace GradeHoraria.Models
{
    public class PeriodoMateria
    {
        public string? Periodo_Id { get; set; }
        public Periodo? Periodo { get; set; }

        public string? Materia_Id { get; set; }
        public Materia? Materia { get; set; }
    }
}