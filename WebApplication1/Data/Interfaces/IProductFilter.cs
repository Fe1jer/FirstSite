using System.Collections.Generic;
using WebApplication1.Data.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Data.Interfaces
{
    public interface IProductFilter
    {
        public string Name { get; }
        public List<FilterCategoryVM> GetFilterCategoriesByProducts(List<Product> products);
        public List<Product> SortProducts(List<Product> products, Dictionary<string, int> filters);
    }
}
