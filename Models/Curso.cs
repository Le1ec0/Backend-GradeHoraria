using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GradeHoraria.Models
{
    public class Curso
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Curso_Id { get; set; }
        public string? Nome { get; set; }
        public string? Turno { get; set; }
        public string? Sala { get; set; }
        public string? Professor { get; set; }
        public virtual ICollection<Periodo> Periodos { get; set; }
        public ICollection<CursoPeriodo> CursoPeriodos { get; set; }
    }
}