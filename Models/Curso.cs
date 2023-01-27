using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GradeHoraria.Models
{
    public class Curso
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Turno { get; set; }
        public string Professor { get; set; }
        public int Periodo { get; set; }
        public List<Periodo> Periodos { get; set; }
        public virtual ICollection<CursoPeriodo> CursoPeriodos { get; set; }
    }
}