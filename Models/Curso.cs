using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GradeHoraria.Models
{
    public class Curso
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? CursoId { get; set; }
        public string? Nome { get; set; }
        public string? Periodo { get; set; }
        public string? Turno { get; set; }
        public string? Sala { get; set; }
        public string? UserId { get; set; }
        [JsonIgnore]
        public virtual ApplicationUser? ApplicationUser { get; set; }
        [JsonIgnore]
        public virtual ICollection<Materia>? Materias { get; set; }
        [JsonIgnore]
        public virtual ICollection<Periodo>? Periodos { get; set; }
    }
}