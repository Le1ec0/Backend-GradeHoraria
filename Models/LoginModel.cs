using System.ComponentModel.DataAnnotations;

namespace GradeHoraria.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "E-mail é necessário.")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Senha é necessária.")]
        public string? Password { get; set; }
    }
}