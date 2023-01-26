using System.ComponentModel.DataAnnotations;

namespace GradeHoraria.Models
{
    public class CursosRequestModel
    {
        [Required]
        public int? Id { get; set; }
        public string? Nome { get; set; }
        public int? Periodo { get; set; }
        public string? Turno { get; set; }
        public string? Sala { get; set; }
        public string? Professor { get; set; }
    }
}