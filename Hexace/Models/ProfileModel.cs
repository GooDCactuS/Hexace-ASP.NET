using System.ComponentModel.DataAnnotations;
using Hexace.Data.Objects;
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

        public User User { get; set; }
        public Profile Profile { get; set; }

        public ProfileModel(User user, Profile profile)
        {
            this.User = user;
            this.Profile = profile;
        }
    }
}