using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GradeHoraria.Models
{
    public class Periodo
    {
        public int? Periodo_Id { get; set; }
        public Curso? Curso { get; set; }
        public int? Curso_Id { get; set; }
        public ICollection<Materia>? Materias { get; set; }
    }
}