using Microsoft.AspNetCore.Identity;

namespace GradeHoraria.Models
{
    public class ApplicationUser : IdentityUser
    {
        public byte[]? PhotoBytes { get; set; }
    }
}
