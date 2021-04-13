using System;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Data.AbstractClasses;
using WebApplication1.Validation;

namespace WebApplication1.Data.Models
{
    public class User : Entity
    {
        [Display(Name = "Электронная почта")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Введите существующую почту")]
        public string Email { get; set; }

        [Required]
        public bool EmailConfirmed { get; set; }

        public virtual DateTimeOffset? LockoutEnd { get; set; }


        [Display(Name = "Пароль")]
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
