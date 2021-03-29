using System.Collections.Generic;
using WebApplication1.Data.Models;

namespace WebApplication1.ViewModels
{
    public class ShowProductViewModel
    {
        public Product Product { get; set; }
        public List<ShopCartItem> ShopCartItems { get; set; }
        public Dictionary<string, int> Filter { get; set; }
    }
}
