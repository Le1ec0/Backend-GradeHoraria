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
        public string Professor { get; set; }
        public int Cursos_Id { get; set; }
        public int Periodo_Id { get; set; }
        public CursoPeriodo CursoPeriodo { get; set; }
        public virtual ICollection<PeriodoMateria> PeriodoMaterias { get; set; }
    }
}