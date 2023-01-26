using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GradeHoraria.Models
{
    public class Materia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
        public string? Nome { get; set; }
        public string? DSemana { get; set; }
        public string? Professor { get; set; }
        [ForeignKey("Periodos")]
        public int? Periodo_Id { get; set; }
        [ForeignKey("Cursos")]
        public int? Curso_Id { get; set; }
        public Periodo? Periodos { get; set; }
        public Curso? Cursos { get; set; }
    }
}