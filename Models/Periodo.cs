using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GradeHoraria.Models
{
    public class Periodo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Sala { get; set; }
        public int CursoId { get; set; }
        public Curso Curso { get; set; }
        public List<Materia> Materias { get; set; }
    }
}