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
        public string? Description { get; set; }
        public int UserId { get; set; }
        public Users? Users { get; set; }
    }
}