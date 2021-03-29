
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Validation;

namespace WebApplication1.ViewModels
{
    public class ProfileViewModel
    {
        [Display(Name = "Электронная почта")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Введите существующую почту")]
        public string Email { get; set; }

        [Display(Name = "Имя")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Введите имя")]
        public string Name { get; set; }

        [Display(Name = "Фамилия")]
        [DataType(DataType.Text)]
        public string Surname { get; set; }

        [Display(Name = "Отчество")]
        [DataType(DataType.Text)]
        public string Patronymic { get; set; }

        [Display(Name = "Номер телефона")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Пол")]
        [DataType(DataType.Text)]
        public string Gender { get; set; }

        [Display(Name = "Дата рождения")]
        [DataType(DataType.Date)]
        [ValidateDateRange]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Изображение")]
        [DataType(DataType.ImageUrl)]
        public string Avatar { get; set; }

        [Display(Name = "Изображение")]
        [Required(ErrorMessage = "Введите существующую почту")]
        public IFormFile Img { get; set; }
    }
}
