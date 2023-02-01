using System.ComponentModel.DataAnnotations;

namespace GradeHoraria.Models
{
    public class ChangeRoleModel
    {
        public string UserName { get; set; }
        public string RoleName { get; set; }
    }
}