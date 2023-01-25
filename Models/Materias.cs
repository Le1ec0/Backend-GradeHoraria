using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GradeHoraria.Models
{
    public class Materias
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? MateriaId { get; set; }
        public string? Nome { get; set; }
        public string? Turno { get; set; }
        public string? DSemana { get; set; }
        public string? Sala { get; set; }
        public string? Professor { get; set; }
        public int? CursoId { get; set; }
        public string? UserId { get; set; }
        [JsonIgnore]
        public virtual ApplicationUser? ApplicationUser { get; set; }
        [JsonIgnore]
        public virtual Cursos? Cursos { get; set; }
    }
}