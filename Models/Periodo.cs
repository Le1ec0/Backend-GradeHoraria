using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GradeHoraria.Models
{
    public class Periodo
    {
        [Key]
        public int Id { get; set; }
        public int PeriodoId { get; set; }
        public int CursoId { get; set; }
        public Curso Cursos { get; set; }
        public ICollection<Materia> Materias { get; set; }
    }
}