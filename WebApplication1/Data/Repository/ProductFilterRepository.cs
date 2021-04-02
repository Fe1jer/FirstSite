using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Data.Repository
{
    public class ProductFilterRepository : IProductFilter
    {
        private readonly IProductRepository products;
        private List<FilterCategoryVM> filter;

        public ProductFilterRepository(IProductRepository products)
        {
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
        public List<FilterCategoryVM> GetFilterCategoriesByProducts(List<Product> products)
        {
            this.filter = new List<FilterCategoryVM>();
            IEnumerable<string> Categories = products.Select(i => i.Category).Distinct().ToList();
            IEnumerable<string> Countries = products.Select(i => i.Country).Distinct().ToList();
            IEnumerable<string> Companies = products.Select(i => i.Company).Distinct().ToList();
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

        public List<Product> SortProducts(List<Product> products, Dictionary<string, int> filters)
        {
            int categoryId = 0;
            List<List<string>> filter = new List<List<string>>() { new List<string>(), new List<string>(), new List<string>() };
            foreach (KeyValuePair<string, int> item in filters)
            {
                if (item.Value != 0)
                {
                    categoryId = Convert.ToInt32(item.Key.Split('-')[0]) - 1;
                    int filterId = item.Value - 1;
                    string i = GetFilterCategoriesByProducts(products)[categoryId].Selections[filterId].Name;
                    if (categoryId == 0)
                    {
                        filter[0].Add(i);
                    }
                    else if (categoryId == 1)
                    {
                        filter[1].Add(i);
                    }
                    else
                    {
                        filter[2].Add(i);
                    }
                }
                else
                {
                    filters.Clear();
                    break;
                }
            }
            categoryId = 0;
            foreach (List<string> item in filter)
            {
                if (item.Count != 0)
                {
                    if (categoryId == 0)
                    {
                        products = products.Where(i => item.Contains(i.Category)).ToList();
                    }
                    else if (categoryId == 1)
                    {
                        products = products.Where(i => item.Contains(i.Company)).ToList();
                    }
                    else
                    {
                        products = products.Where(i => item.Contains(i.Country)).ToList();
                    }
                }
                categoryId++;
            }
            return products;
        }

    }
}
