using System.Collections.Generic;

namespace WebApplication1.ViewModels
{
    public class ProductsListViewModel
    {
        public IEnumerable<ShowProductViewModel> AllProducts { get; set; }
        public List<string> Filter { get; set; }
        public List<FilterCategoryVM> FilterSort { get; set; }
    }
}
