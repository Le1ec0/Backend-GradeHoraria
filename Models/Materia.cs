using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GradeHoraria.Models
{
    public class Materia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Materia_Id { get; set; }
        public string Nome { get; set; }
        public string DSemana { get; set; }
        public string Professor { get; set; }
        public ICollection<PeriodoMateria> PeriodoMaterias { get; set; }
    }
}