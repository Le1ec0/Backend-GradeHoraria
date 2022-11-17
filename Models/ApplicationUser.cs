using System.ComponentModel.DataAnnotations;

namespace Backend_CarStore.Models
{
    public class ApplicationUser
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "User Name is required")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }
        public int? Phone { get; set; }

    }
}