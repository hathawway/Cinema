using System.ComponentModel.DataAnnotations;

namespace Cinema.Models.User
{
    public class UserViewModel
    {

        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
