using System.Collections.Generic;
using WebApplication1.Data.Models;

namespace WebApplication1.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<ShowProductViewModel> FavProducts { get; set; }
        public IEnumerable<CaruselItem> CaruselItems { get; set; }
        public IEnumerable<News> NewsList { get; set; }
    }
}
