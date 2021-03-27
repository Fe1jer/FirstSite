using System.Collections.Generic;
using System.Linq;
using WebApplication1.Data.Interfaces;
using WebApplication1.ViewModels;

namespace WebApplication1.Data.Repository
{
    public class ProductFilterRepository : IProductFilter
    {
        private readonly AppDBContext appDBContent;
        private readonly IProductRepository products;
        private List<FilterCategoryVM> filter;

        public ProductFilterRepository(AppDBContext appDBContent, IProductRepository products)
        {
            this.appDBContent = appDBContent;
            this.products = products;
        }
        public string Name { get => "Фильтр продуктов"; }
        public List<FilterCategoryVM> FilterCategories
        {
            get
            {
                this.filter = new List<FilterCategoryVM>();
                IEnumerable<string> Categories = products.GetProductsAsync().Result.Select(i => i.Category).Distinct().ToList();
                IEnumerable<string> Countries = products.GetProductsAsync().Result.Select(i => i.Country).Distinct().ToList();
                IEnumerable<string> Companies = products.GetProductsAsync().Result.Select(i => i.Company).Distinct().ToList();
                Dictionary<string, IEnumerable<string>> filter = new Dictionary<string, IEnumerable<string>>() { { "Категория", Categories }, { "Компания", Companies }, { "Страна производитель", Countries } };

                int categoryId = 1;
                foreach (KeyValuePair<string, IEnumerable<string>> item in filter)
                {
                    FilterCategoryVM category = new FilterCategoryVM
                    {
                        ID = categoryId,
                        Name = item.Key,
                        Selections = new List<FilterSelectionVM>()
                    };
                    int selectionId = 1;
                    foreach (string i in item.Value)
                    {
                        FilterSelectionVM filterSelection = new FilterSelectionVM
                        {
                            ID = selectionId,
                            Name = i
                        };
                        category.Selections.Add(filterSelection);
                        selectionId++;
                    }
                    this.filter.Add(category);
                    categoryId++;
                }
                return this.filter;
            }
        }
    }
}
