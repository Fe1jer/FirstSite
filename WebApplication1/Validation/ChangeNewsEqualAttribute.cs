using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.ViewModels;

namespace WebApplication1.Validation
{
    public class ChangeNewsEqualAttribute : ValidationAttribute
    {
        public ChangeNewsEqualAttribute()
        {
            ErrorMessage = "Заполните все поля!";
        }
        public override bool IsValid(object value)
        {
            ChangeNewsViewModel model = value as ChangeNewsViewModel;

            if (model.IsCaruselNews == true && model.FavImg == null && model.FileFavImg == null)
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

            return true;
        }

        public bool ThereImg(string path)
        {
            if (File.Exists($"wwwroot{path}"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
