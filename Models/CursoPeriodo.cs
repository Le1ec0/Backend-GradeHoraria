using System.ComponentModel.DataAnnotations;

namespace GradeHoraria.Models
{
    public class CursoPeriodo
    {
        public string? Curso_Id { get; set; }
        public Curso? Curso { get; set; }
        public string? Periodo_Id { get; set; }
        public Periodo? Periodo { get; set; }
    }
}