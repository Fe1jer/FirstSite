using System;
using System.ComponentModel.DataAnnotations;
using InternetShop.Validation;
using Microsoft.AspNetCore.Identity;

namespace InternetShop.Data.Models
{
    public class User : IdentityUser<int>
    {
        [Display(Name = "Имя")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Display(Name = "Фамилия")]
        [DataType(DataType.Text)]
        public string Surname { get; set; }

        [Display(Name = "Отчество")]
        [DataType(DataType.Text)]
        public string Patronymic { get; set; }

        [Display(Name = "Пол")]
        [DataType(DataType.Text)]
        public string Gender { get; set; }

        [Display(Name = "Дата рождения")]
        [DataType(DataType.Date)]
        [ValidateDateRange]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Изображение")]
        public string Img { get; set; }
    }
}
