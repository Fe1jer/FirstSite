using System.ComponentModel.DataAnnotations;
using System.IO;
using InternetShop.ViewModels;

namespace InternetShop.Validation
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

            if (model.IsCaruselNews == true && (model.FavImg == null || ThereImg("/img/news/" + model.FavImg.FileName)))
            {
                return false;
            }
            if (model.IsProductHref == true && model.ProductHref == null)
            {
                return false;
            }
            else if (model.IsProductHref == false && model.Text == null)
            {
                return false;
            }
            if (!ThereImg("/img/news/" + model.Img.FileName))
            {
                return false;
            }
            return true;
        }

        public bool ThereImg(string path)
        {
            if (File.Exists($"wwwroot{path}"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
