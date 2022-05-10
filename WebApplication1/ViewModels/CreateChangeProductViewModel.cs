using InternetShop.Data.Models;
using InternetShop.Validation;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace InternetShop.ViewModels
{
    public class CreateChangeProductViewModel
    {
        public Product Product { get; set; }

        [Display(Name = "Изображение")]
        [ValidateImages]
        public IFormFileCollection Uploads { get; set; }
    }
}
