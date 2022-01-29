using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using InternetShop.Data.AbstractClasses;

namespace InternetShop.Data.Models
{
    public class Product : Entity
    {
        public ProductType ProductType { get; set; }

        [Display(Name = "Краткое описание")]
        [Required(ErrorMessage = "Введите дествительное описание")]
        public string ShortDesc { set; get; }

        [Display(Name = "Картинка (url или /img/...) 16x9")]
        [DataType(DataType.ImageUrl)]
        [Required(ErrorMessage = "Введите дествительную картинку")]
        public string Img { set; get; }

        [Display(Name = "Страна производитель")]
        [Required(ErrorMessage = "Введите действительную страну")]
        public string Country { set; get; }

        [Display(Name = "Цена")]
        [Required(ErrorMessage = "Введите дествительную цену")]
        public ushort Price { set; get; }

        [Display(Name = "Количество")]
        public int Count { set; get; }

        [Display(Name = "Характеристики")]
        public List<AttributeValue> AttributeValues { set; get; }
    }
}