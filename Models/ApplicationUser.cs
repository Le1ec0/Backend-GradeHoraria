using Microsoft.AspNetCore.Identity;

namespace Backend_CarStore.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int CarId { get; set; }
        public Cars Cars { get; set; }
    }
}