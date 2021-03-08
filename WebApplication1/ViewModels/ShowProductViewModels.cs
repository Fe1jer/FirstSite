using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data.Models;

namespace WebApplication1.ViewModels
{
    public class ShowProductViewModels
    {
        public Product Product { get; set; }
        public List<ShopCartItem> ShopCartItems { get; set; }
        public ProductFilter Filter { get; set; }
    }
}
