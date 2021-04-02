using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Validation
{
    public class ValidateDateRange : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime? dt = (DateTime?)value;

            if (dt == null || dt <= DateTime.Now)
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
