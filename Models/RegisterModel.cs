using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GradeHoraria.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Insira o nome do usuário.")]
        [Description("Insira o nome do usuário.")]
        public string? displayName { get; set; }

        [Required(ErrorMessage = "Insira o apelido do usuário.")]
        [Description("Insira o apelido do usuário.")]
        public string? mailNickname { get; set; }

        [Required(ErrorMessage = "Insira o e-mail do usuário.")]
        [Description("Insira o e-mail do usuário.")]
        public string? userPrincipalName { get; set; }

        [Required(ErrorMessage = "Insira a senha do usuário.")]
        [Description("Insira a senha do usuário.")]
        public string? password { get; set; }
    }
}