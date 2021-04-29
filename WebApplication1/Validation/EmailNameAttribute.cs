using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebApplication1.Validation
{
    public class EmailNameAttribute : ValidationAttribute
    {
        //массив для хранения недопустимых имен
        string[] _email;

        public EmailNameAttribute(string[] email)
        {
            _email = email;
        }
        public override bool IsValid(object value)
        {
            if (value != null && !_email.Contains(value.ToString()))
            {
                return true;
            }

            return false;
        }
    }
}
