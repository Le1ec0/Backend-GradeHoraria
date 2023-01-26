using System.ComponentModel.DataAnnotations;

namespace GradeHoraria.Models
{
    public class Periodo
    {
        [Key]
        public string? Materias_Nome { get; set; }
        public string? Materias_Semestre { get; set; }
        public int? Cursos_Id { get; set; }
        public virtual Curso? Cursos { get; set; }
        public virtual ICollection<Materia>? Materias { get; set; }
    }
}