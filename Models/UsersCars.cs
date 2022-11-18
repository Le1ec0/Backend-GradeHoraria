using System.ComponentModel.DataAnnotations.Schema;

namespace CarStore.Models
{
    public class UsersCars
    {
        public int UserId { get; set; }
        public int CarId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("CarId")]
        public Cars Cars { get; set; }
    }
}