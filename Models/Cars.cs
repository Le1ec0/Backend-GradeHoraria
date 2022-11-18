using System.ComponentModel.DataAnnotations;

namespace CarStore.Models
{
    public class Cars
    {
        [Key]
        public string CarId { get; set; }
        public string Plate { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public int? Year { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        /*public virtual ApplicationUser ApplicationUser { get; set; }*/
    }
}