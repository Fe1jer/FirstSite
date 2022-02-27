using InternetShop.Data.Models;

namespace InternetShop.ViewModels
{
    public class ShowProductViewModel
    {
        public Product Product { get; set; }
        public bool IsInCart { get; set; }
        public bool IsAvailable{ get; set; }
    }
}
