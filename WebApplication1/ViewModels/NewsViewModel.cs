using System.Collections.Generic;
using InternetShop.Data.Models;

namespace InternetShop.ViewModels
{
    public class NewsViewModel
    {
        public IEnumerable<ShowProductViewModel> FavProducts { get; set; }
        public IEnumerable<CaruselItem> CaruselItems { get; set; }
        public IEnumerable<News> NewsList { get; set; }
    }
}
