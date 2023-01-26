using System.ComponentModel.DataAnnotations;

namespace GradeHoraria.Models
{
    public class MateriasRequestModel
    {
        public string? Nome { get; set; }
        public string? DSemana { get; set; }
        public string? Professor { get; set; }
        public int? Cursos_Id { get; set; }
        public int? Periodo_Id { get; set; }
    }
}