using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Validation
{
    public class ValidateDateRange : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime? dt = (DateTime?)value;

            if (dt <= DateTime.Now.AddDays(-5).AddHours(-3) || dt == null)
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
