using System.Collections.Generic;
using WebApplication1.Data.Models;

namespace WebApplication1.ViewModels
{
    public class ProductsListViewModel
    {
        public IEnumerable<Product> AllProducts { get; set; }
        public ShopCart ShopCart { get; set; }
        public string[] Filter { get; set; }
        public string[] FilterSort { get; set; }
    }
}
