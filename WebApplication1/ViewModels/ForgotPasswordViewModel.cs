using System.ComponentModel.DataAnnotations;

namespace InternetShop.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
