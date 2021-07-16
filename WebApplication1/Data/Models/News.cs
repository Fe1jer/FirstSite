using System;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Data.AbstractClasses;

namespace WebApplication1.Data.Models
{
    public class News : Entity
    {
        [Display(Name = "Название новости")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Введите название")]
        public string Name { get; set; }

        [Display(Name = "Изображение")]
        [Required(ErrorMessage = "Выберите изображение")]
        [DataType(DataType.ImageUrl)]
        public string Img { get; set; }

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

        public string ProductHref { get; set; }
        public DateTime CreateData { get; set; }
    }
}
