using System.ComponentModel.DataAnnotations;

namespace GradeHoraria.Models
{
    public class ChangeRoleModel : UserRoles
    {
        public string Email { get; set; }
        public string RoleName { get; set; }
    }
}