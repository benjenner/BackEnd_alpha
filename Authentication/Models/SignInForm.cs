using System.ComponentModel.DataAnnotations;

namespace Authentication.Models
{
    public class SignInForm
    {
        // Required-fält behövs för swagger
        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}