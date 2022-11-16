using System.ComponentModel.DataAnnotations;

namespace Backend_CarStore.Models
{
    public class Cars
    {
        [Key]
        public int Id { get; set; }
        public string? Plate { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string? Color { get; set; }
        public int? Year { get; set; }
        public int? Description { get; set; }
        public string? ImageTitle { get; set; }
    }
}