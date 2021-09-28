using System.Collections.Generic;
using InternetShop.Data.Models;

namespace InternetShop.ViewModels
{
    public class OrderViewModel
    {
        public Order Order { get; set; }
        public List<ShopCartItem> ShopCartItems { get; set; }
    }
}
