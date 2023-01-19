using System.ComponentModel.DataAnnotations;

namespace GradeHoraria.Models
{
    public class Cursos
    {
        [Key]
        public int CursoId { get; set; }
        public string Nome { get; set; }
        public string Disciplina { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<Materias> Materias { get; set; }
    }
}