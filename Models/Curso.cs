using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GradeHoraria.Models
{
    public class Curso
    {
        public int? Curso_Id { get; set; }
        public string? Nome { get; set; }
        public string? Turno { get; set; }
        public string? Sala { get; set; }
        public string? Professor { get; set; }
        public ICollection<Periodo>? Periodos { get; set; }
    }
}