using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public byte[]? Photos { get; set; }

    }
    public class Photos
    {
        [Key]
        [ForeignKey("Id")]
        public Guid Id { get; set; }
        public byte[]? Bytes { get; set; }
        public string? Description { get; set; }
        public string? FileExtension { get; set; }
        public decimal Size { get; set; }
        public Cars? Cars { get; set; }
    }
}