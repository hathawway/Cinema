using System.ComponentModel.DataAnnotations;

namespace Cinema.Models.User
{
    public class NewUserViewModel
    {
        /// <summary>
        /// Почта
        /// </summary>
        [Required]
        [Display(Name = "Логин")]
        public string Login { get; set; }
        /// <summary>
        /// Логин
        /// </summary>
        [Required]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }
        /// <summary>
        /// Фамилия
        /// </summary>
        [Required]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        /// <summary>
        /// Повторение пароля
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают!")]
        public string ConfirmPassword { get; set; }
    }
}
