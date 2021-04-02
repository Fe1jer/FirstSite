using System.Collections.Generic;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Models;

namespace WebApplication1.ViewModels
{
    public class ProductsListViewModel
    {
        public IEnumerable<Product> AllProducts { get; set; }
        public List<ShopCartItem> ShopCartItems { get; set; }
        public Dictionary<string, int> Filter { get; set; }
        public List<FilterCategoryVM> FilterSort { get; set; }
    }
}
