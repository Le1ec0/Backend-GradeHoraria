using System.ComponentModel.DataAnnotations;

namespace GradeHoraria.Models
{
    public class PeriodoMateria
    {
        [Key]
        public int Id { get; set; }
        public int Materia_Id { get; set; }
        public Materia Materia { get; set; }
        public int Periodo_Id { get; set; }
        public Periodo Periodo { get; set; }
    }
}