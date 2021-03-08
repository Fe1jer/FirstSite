using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Data.AbstractClasses;

namespace WebApplication1.Data.Models
{
    public class Order : Entity
    {

        public User Courier { get; set; }

        [Display(Name = "Имя")]
        [StringLength(10)]
        [Required(ErrorMessage = "Введите имя")]
        public string Name { get; set; }

        [Display(Name = "Фамилия")]
        [StringLength(10)]
        [Required(ErrorMessage = "Введите фамилию")]
        public string Surname { get; set; }

        [Display(Name = "Адрес")]
        [StringLength(40, MinimumLength = 7)]
        [Required(ErrorMessage = "Длина адреса не менее 7 символов")]
        public string Adress { get; set; }

        [Display(Name = "Номер телефона")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(13, MinimumLength = 3)]
        [Required(ErrorMessage = "Длина номера не менее 7 символов")]
        public string Phone { get; set; }

        [Display(Name = "Электронная почта")]
        [DataType(DataType.EmailAddress)]
        [StringLength(25, MinimumLength = 7)]
        [Required(ErrorMessage = "Длина электронная почты не менее 7 символов")]
        public string Email { get; set; }

        [Display(Name = "Имя пользователя")]
        [StringLength(10)]
        [Required(ErrorMessage = "Введите действительное имя пользователя")]
        public string UserName { get; set; }

        [Display(Name = "Почтовый индекс")]
        [StringLength(6, MinimumLength = 6)]
        [Required(ErrorMessage = "Длина 6 символов")]
        public string Zip { get; set; }

/*        [Display(Name = "Имя на карте")]
        [DataType(DataType.CreditCard)]
        [StringLength(25, MinimumLength = 7)]
        [Required(ErrorMessage = "Длина не менее 7 символов")]*/
        public string CCName { get; set; }

/*        [Display(Name = "Номер карты")]
        [DataType(DataType.CreditCard)]
        [StringLength(16, MinimumLength = 16)]
        [Required(ErrorMessage = "Длина не менее 16 символов")]*/
        public string CCNumber { get; set; }

/*        [Display(Name = "Срок действия")]
        [DataType(DataType.CreditCard)]
        [StringLength(25, MinimumLength = 7)]
        [Required(ErrorMessage = "Длина не менее 7 символов")]*/
        public string CCExpiration { get; set; }

/*        [Display(Name = "CVV")]
        [DataType(DataType.CreditCard)]
        [StringLength(3, MinimumLength = 3)]
        [Required(ErrorMessage = "Длина не менее 3 символов")]*/
        public string CCCVV { get; set; }

        [BindNever]
        [ScaffoldColumn(false)]
        public DateTime OrderTime { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }
    }
}
