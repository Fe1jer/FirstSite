using System.ComponentModel.DataAnnotations;
using WebApplication1.ViewModels;

public class NamePasswordEqualAttribute : ValidationAttribute
{
    public NamePasswordEqualAttribute()
    {
        ErrorMessage = "Имя и пароль не должны совпадать!";
    }
    public override bool IsValid(object value)
    {
        RegisterViewModel p = value as RegisterViewModel;

        if (p.Name == p.Password)
        {
            return false;
        }
        return true;
    }
}