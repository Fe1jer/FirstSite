using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace InternetShop.Validation
{
    public class ValidateImages : ValidationAttribute
    {
        public static readonly string[] mimeTypes = new[] { "image/jpeg", "image/png" };
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            IFormFileCollection dt = (IFormFileCollection)value;
            if (dt != null)
            {
                foreach (var d in dt)
                {
                    if (!mimeTypes.Contains(d.ContentType))
                    {
                        return new ValidationResult("Выберите изображение");
                    }
                }
            }
            return ValidationResult.Success;
        }
    }
}
