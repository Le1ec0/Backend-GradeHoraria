using System.ComponentModel.DataAnnotations;

namespace GradeHoraria.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string? displayName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? password { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? mailNickname { get; set; }
    }
}