using System.Collections.Generic;
using WebApplication1.ViewModels;

namespace WebApplication1.Data.Interfaces
{
    public interface IProductFilter
    {
        public string Name { get; }
        public List<FilterCategoryVM> FilterCategories { get; }
    }
}
