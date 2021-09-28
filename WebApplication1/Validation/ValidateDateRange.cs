using System;
using System.ComponentModel.DataAnnotations;

namespace InternetShop.Validation
{
    public class ValidateDateRange : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime? dt = (DateTime?)value;

            if (dt == null || (dt <= DateTime.Now && dt >= DateTime.Now.AddYears(-100)))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Введите верную дату рождения");
            }
        }
    }
}
