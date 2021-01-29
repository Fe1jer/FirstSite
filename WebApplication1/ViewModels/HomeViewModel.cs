using System.Collections.Generic;
using WebApplication1.Data.Models;

namespace WebApplication1.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Product> FavProducts { get; set; }
        public IEnumerable<HomeProduct> AutoOnTheCover { get; set; }
    }
}
