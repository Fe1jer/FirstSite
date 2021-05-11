using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Validation;

namespace WebApplication1.ViewModels
{
    [ChangeNewsEqual]
    public class ChangeNewsViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Название новости")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Введите название")]
        public string Name { get; set; }

        [Display(Name = "Изображение")]
        [DataType(DataType.ImageUrl)]
        public string Img { get; set; }

        [ValidateImg]
        public IFormFile FileImg { get; set; }

        [Display(Name = "Краткое описание новости")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Введите описание")]
        public string Desc { get; set; }

        [Display(Name = "Суть новости")]
        [DataType(DataType.Text)]
        public string Text { get; set; }

        [Display(Name = "Изображение в карусели")]
        [DataType(DataType.ImageUrl)]
        public string FavImg { get; set; }

        [ValidateImg]
        public IFormFile FileFavImg { get; set; }

        public string ProductHref { get; set; }

        [Display(Name = "Является ссылкой на товар")]
        public bool IsProductHref { get; set; }

        [Display(Name = "Является элементом карусели")]
        public bool IsCaruselNews { get; set; }
    }
}
