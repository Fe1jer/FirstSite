using System.ComponentModel.DataAnnotations;
using InternetShop.Data.AbstractClasses;

namespace InternetShop.Data.Models
{
    public class ProductType : Entity
    {
        [Display(Name = "Название")]
        [Required(ErrorMessage = "Введите дествительное название")]
        public string Name { set; get; }

        [Display(Name = "Категория")]
        [Required(ErrorMessage = "Введите дествительную категорию")]
        public string Category { set; get; }

        [Display(Name = "Компания")]
        [Required(ErrorMessage = "Введите дествительную компанию")]
        public string Company { set; get; }
    }
}
