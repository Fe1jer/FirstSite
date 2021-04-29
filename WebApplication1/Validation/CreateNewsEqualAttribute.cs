using System.ComponentModel.DataAnnotations;
using WebApplication1.ViewModels;

namespace WebApplication1.Validation
{
    public class CreateNewsEqualAttribute : ValidationAttribute
    {
        public CreateNewsEqualAttribute()
        {
            ErrorMessage = "Заполните все поля!";
        }
        public override bool IsValid(object value)
        {
            CreateNewsViewModel model = value as CreateNewsViewModel;

            if (model.IsCaruselNews == true && model.FavImg == null)
            {
                return false;
            }
            if (model.IsProductHref == true && model.ProductId == null)
            {
                return false;
            }
            else if (model.IsProductHref == false && model.Text == null)
            {
                return false;
            }
            return true;
        }
    }
}
