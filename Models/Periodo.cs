using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GradeHoraria.Models
{
    public class Periodo
    {
        [Key]
        [ForeignKey("Periodos")]
        public ICollection<int>? Periodo_Id { get; set; }
        [ForeignKey("Cursos")]
        public int? Cursos_Id { get; set; }
        public Curso? Cursos { get; set; }
        [JsonIgnore]
        public virtual ICollection<Materia>? Materias { get; set; }
        [JsonIgnore]
        public virtual ICollection<CursoPeriodo>? CursoPeriodos { get; set; }
    }
}