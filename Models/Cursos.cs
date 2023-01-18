namespace GradeHoraria.Models
{
    public class Cursos
    {
        public int? CursoId { get; set; }
        public string? Nome { get; set; }
        public string? Disciplina { get; set; }
        public int ApplicationUserId { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }
        public virtual ICollection<Materias>? Materias { get; set; }
    }
}