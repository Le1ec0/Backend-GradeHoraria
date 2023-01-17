namespace GradeHoraria.Models
{
    public class Materias
    {
        public int? MateriaId { get; set; }
        public string? Plate { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string? Color { get; set; }
        public int? Year { get; set; }
        public string? Description { get; set; }
        public int? UserId { get; set; }
        public ICollection<ApplicationUser>? ApplicationUsers { get; set; }
    }
}