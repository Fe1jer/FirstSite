using System.ComponentModel.DataAnnotations;

namespace InternetShop.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "Электронный адрес", Prompt = "Электронный адрес")]
        [Required(ErrorMessage = "Не указан Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Пароль", Prompt = "Пароль")]
        [Required(ErrorMessage = "Длина пароля не менее 6 символов")]
        [DataType(DataType.Password)]
        [StringLength(25, MinimumLength = 6)]
        public string Password { get; set; }

        [Display(Name = "Запомнить?")]
        public bool RememberMe { get; set; }
    }
}
