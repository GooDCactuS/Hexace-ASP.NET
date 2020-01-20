using System.ComponentModel.DataAnnotations;

namespace Hexace.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Nickname not specified")]
        public string Nickname { get; set; }

        [Required(ErrorMessage = "Email not specified")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password not specified")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password configramtion is not specified")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string PasswordConfirmation { get; set; }
    }
}