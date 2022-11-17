using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Backend_CarStore.Models
{
    public class Cars
    {
        [Key]
        public int CarId { get; set; }
        public string Plate { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public int? Year { get; set; }
        public string Description { get; set; }
        public IEnumerable<ApplicationUser> ApplicationUsers { get; set; }
    }
}