namespace GradeHoraria.Models
{
    public class Materias
    {
        public int? MateriaId { get; set; }
        public string? Nome { get; set; }
        public int? UserId { get; set; }
        public IEnumerable<Cursos>? Cursos { get; set; }
        public IEnumerable<ApplicationUser>? ApplicationUsers { get; set; }
    }
}