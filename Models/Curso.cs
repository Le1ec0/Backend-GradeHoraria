using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GradeHoraria.Models
{
    public class Curso
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
        public string? Nome { get; set; }
        public int? Periodo_Id { get; set; }
        public string? Turno { get; set; }
        public string? Sala { get; set; }
        public string? Professor { get; set; }
        [JsonIgnore]
        public virtual ICollection<Materia>? Materias { get; set; }
        [JsonIgnore]
        public virtual ICollection<Periodo>? Periodos { get; set; }
    }
}