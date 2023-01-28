using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GradeHoraria.Models
{
    public class Materia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string DSemana { get; set; }
        public string Sala { get; set; }
        public string Professor { get; set; }
        public int CursoId { get; set; }
        public int PeriodoId { get; set; }
        public Curso Curso { get; set; }
        public Periodo Periodo { get; set; }
    }
}