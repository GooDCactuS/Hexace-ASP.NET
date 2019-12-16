using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hexace.Models
{
    public class ProfileModel
    {

        [Required(ErrorMessage = "Nickname not specified")]
        public string Nickname { get; set; }

        [Required(ErrorMessage = "Email not specified")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password not specified")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string PasswordConfirmation { get; set; }

    }
}