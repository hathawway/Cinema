using System.ComponentModel.DataAnnotations;

namespace Cinema.Models.User
{
    public class UserViewModel
    {
        public long Kod { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public IdName Role { get; set; }
    }
}
