using System.Collections.Generic;
using WebApplication1.Data.Models;

namespace WebApplication1.ViewModels
{
    public class OrderViewModel
    {
        public Order Order { get; set; }
        public List<ShopCartItem> ShopCartItems { get; set; }
    }
}
