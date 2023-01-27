using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GradeHoraria.Models
{
    public class Materia
    {
        public int? Id { get; set; }
        public string? Nome { get; set; }
        public string? DSemana { get; set; }
        public string? Professor { get; set; }
        public Periodo? Periodo { get; set; }
        public int? Periodo_Id { get; set; }
    }
}