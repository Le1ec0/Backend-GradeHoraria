using Microsoft.AspNetCore.Identity;

namespace GradeHoraria.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Role { get; set; }
    }
}