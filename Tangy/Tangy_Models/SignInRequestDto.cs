using System.ComponentModel.DataAnnotations;

namespace Tangy_Models
{
    public class SignInRequestDto
    {
        [Required(ErrorMessage = "Username is required.")]
        [EmailAddress(ErrorMessage = "Username must be a valid email address.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
