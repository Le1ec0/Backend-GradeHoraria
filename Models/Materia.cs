using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GradeHoraria.Models
{
    public class Materia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Materia_Id { get; set; }
        public string? Nome { get; set; }
        public string? DSemana { get; set; }
        public string? Professor { get; set; }
        public virtual Curso? Curso { get; set; }
        [ForeignKey("Curso_Id")]
        public int Curso_Id { get; set; }
        public virtual Periodo? Periodo { get; set; }
        [ForeignKey("Periodo_Id")]
        public int[]? Periodo_Id { get; set; }
    }
}