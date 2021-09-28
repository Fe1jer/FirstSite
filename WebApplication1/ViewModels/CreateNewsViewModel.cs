using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using InternetShop.Validation;

namespace InternetShop.ViewModels
{
    [CreateNewsEqual]
    public class CreateNewsViewModel
    {
        [Display(Name = "Название новости")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Введите название")]
        public string Name { get; set; }

        [Display(Name = "Изображение (желательно 16x9)")]
        [Required(ErrorMessage = "Выберите изображение")]
        [ValidateImg]
        public IFormFile Img { get; set; }

        [Display(Name = "Краткое описание новости")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Введите описание")]
        public string Desc { get; set; }

        [Display(Name = "Суть новости")]
        [DataType(DataType.Text)]
        public string Text { get; set; }

        [Display(Name = "Изображение в карусели (желательно 5x2)")]
        [ValidateImg]
        public IFormFile FavImg { get; set; }

        public string ProductHref { get; set; }

        [Display(Name = "Является ссылкой на товар")]
        public bool IsProductHref { get; set; }

        [Display(Name = "Является элементом карусели")]
        public bool IsCaruselNews { get; set; }

        public DateTime CreateData { get; set; }
    }
}
