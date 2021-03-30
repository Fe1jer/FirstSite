using WebApplication1.Data.AbstractClasses;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Validation;
using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Data.Models
{
    public class User : Entity
    {
        [Display(Name = "Электронная почта")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Введите существующую почту")]
        public string Email { get; set; }

        [Display(Name = "Пароль")]/*
        [DataType(DataType.Password)]
        [StringLength(25, MinimumLength = 6)]
        [Required(ErrorMessage = "Длина пароля не менее 6 символов")]*/
        public string Password { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }

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
        public string Img { get; set; }
    }
}
