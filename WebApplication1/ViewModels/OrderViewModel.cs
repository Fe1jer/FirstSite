using WebApplication1.Data.Models;
using System.Collections.Generic;

namespace WebApplication1.ViewModels
{
    public class OrderViewModel
    {
        public Order Order { get; set; }
        public List<ShopCartItem> ShopCartItems { get; set; }
    }
}
