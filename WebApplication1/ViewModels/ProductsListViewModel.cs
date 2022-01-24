using ReflectionIT.Mvc.Paging;
using System.Collections.Generic;

namespace InternetShop.ViewModels
{
    public class ProductsListViewModel
    {
        public PagingList<ShowProductViewModel> AllProducts { get; set; }
        public List<string> Filter { get; set; }
        public List<FilterCategoryVM> FilterSort { get; set; }
    }
}
