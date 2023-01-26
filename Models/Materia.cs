using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GradeHoraria.Models
{
    public class Materia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? MateriaId { get; set; }
        public string? Nome { get; set; }
        public string? DSemana { get; set; }
        public string? Professor { get; set; }
        public int? CursoId { get; set; }
        public string? UserId { get; set; }
        public string? Semestre { get; set; }
        public virtual Periodo? Periodos { get; set; }
        [JsonIgnore]
        public virtual ApplicationUser? ApplicationUser { get; set; }
        [JsonIgnore]
        public virtual Curso? Cursos { get; set; }
    }
}