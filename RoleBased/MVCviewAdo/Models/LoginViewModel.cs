using System.ComponentModel.DataAnnotations;

namespace MVCview.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter valid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(4, ErrorMessage = "Minimum 4 characters")]
        public string Password { get; set; }
    }
}
