using Microsoft.AspNetCore.Identity;

namespace CarStore.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? UserId { get; set; }
        public string? CarId { get; set; }
        public Cars? Cars { get; set; }
    }
}