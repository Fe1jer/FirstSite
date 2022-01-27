using System.ComponentModel.DataAnnotations;
using InternetShop.Validation;

namespace InternetShop.ViewModels
{
    [NamePasswordEqual]
    public class RegisterViewModel
    {
        [EmailName(new string[] { "admin", "moder", "courier" }, ErrorMessage = "Недопустимый email")]
        [Display(Name = "Электронный адрес", Prompt = "Электронный адрес")]
        [Required(ErrorMessage = "Не указан Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Пароль", Prompt = "Пароль")]
        [Required(ErrorMessage = "Длина пароля не менее 6 символов")]
        [DataType(DataType.Password)]
        [StringLength(25, MinimumLength = 6)]
        public string Password { get; set; }

        [Display(Name = "Повторите пароль", Prompt = "Повторите пароль")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string PasswordConfirm { get; set; }

        [Display(Name = "Имя")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Введите имя")]
        public string Name { get; set; }

        [Display(Name = "Пол")]
        [DataType(DataType.Text)]
        [ValidateGender]
        public string Gender { get; set; }
    }
}
