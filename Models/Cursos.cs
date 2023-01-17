namespace GradeHoraria.Models
{
    public class Cursos
    {
        public int? CursoId { get; set; }
        public string? Disciplina { get; set; }
        public string? Periodo { get; set; }
        public string? Turno { get; set; }
        public string? DSemana { get; set; }
        public string? Sala { get; set; }
        public string? Professor { get; set; }
        public int? UserId { get; set; }
        public IEnumerable<Materias>? Materias { get; set; }
        public IEnumerable<ApplicationUser>? ApplicationUsers { get; set; }
    }
}