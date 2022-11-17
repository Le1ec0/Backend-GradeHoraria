using System.ComponentModel.DataAnnotations;

namespace Backend_CarStore.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}