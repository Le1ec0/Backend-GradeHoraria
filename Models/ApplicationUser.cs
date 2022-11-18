using Microsoft.AspNetCore.Identity;

namespace CarStore.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int Password { get; set; }
        public int Phone { get; set; }
        /*public int CarId { get; set; }
        public Cars? Cars { get; set; }*/
    }
}