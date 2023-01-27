using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GradeHoraria.Models
{
    public class PeriodoMateria
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Materia_Id")]
        public int Materia_Id { get; set; }
        public Materia Materia { get; set; }
        [ForeignKey("Periodo_Id")]
        public int Periodo_Id { get; set; }
        public Periodo Periodo { get; set; }
    }
}