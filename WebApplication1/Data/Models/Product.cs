using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Data.Models
{
    public class Product
    {
        public int Id { set; get; }

        [Display(Name = "Название")]
        [Required(ErrorMessage = "Введите дествительное название")]
        public string Name { set; get; }

        [Display(Name = "Краткое описание")]
        [Required(ErrorMessage = "Введите дествительное описание")]
        public string ShortDesc { set; get; }

        [Display(Name = "Полное описание")]
        [Required(ErrorMessage = "Введите дествительное описание")]
        public string LongDesc { set; get; }

       [Display(Name = "Картинка (url или /img/...) 16x9")]
        [DataType(DataType.ImageUrl)]
        [Required(ErrorMessage = "Введите дествительную картинку")]
        public string Img { set; get; }

        [Display(Name = "Категория")]
        [Required(ErrorMessage = "Введите дествительную категорию")]
        public string Category { set; get; }

        [Display(Name = "Компания")]
        [Required(ErrorMessage = "Введите дествительную компанию")]
        public string Company { set; get; }

        [Display(Name = "Страна производитель")]
        [Required(ErrorMessage = "Введите действительную страну")]
        public string Country { set; get; }

        [Display(Name = "Цена")]
        [Required(ErrorMessage = "Введите дествительную цену")]
        public ushort Price { set; get; }

        [Display(Name = "Рекомендовать")]
        public bool IsFavourite { set; get; }

        [Display(Name = "Наличие")]
        public bool Available { set; get; }

    }
}