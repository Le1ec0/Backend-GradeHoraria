using System.ComponentModel.DataAnnotations;

namespace GradeHoraria.Models
{
    public class MateriasRequestModel
    {
        [Required]
        public int? Id { get; set; }
        public string? Nome { get; set; }
        public string? DSemana { get; set; }
        public string? Professor { get; set; }
        public int? Cursos_Id { get; set; }
        public string? Semestre { get; set; }
    }
}