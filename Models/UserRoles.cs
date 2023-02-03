using Microsoft.AspNetCore.Identity;

namespace GradeHoraria.Models
{
    public class UserRoles : IdentityRole
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }
}