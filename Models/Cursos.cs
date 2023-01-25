using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GradeHoraria.Models
{
    public class Cursos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? CursoId { get; set; }
        public string? Nome { get; set; }
        public int? Periodo { get; set; }
        public string? UserId { get; set; }

        [JsonIgnore]
        public virtual ApplicationUser? ApplicationUser { get; set; }
        [JsonIgnore]
        public virtual ICollection<Materias>? Materias { get; set; }
    }
}