using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Models;
using WebApplication1.Data.Specifications;
using WebApplication1.Data.Specifications.Base;
using WebApplication1.ViewModels;

namespace WebApplication1.Data.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly IShopCart _shopCart;

        public ProductRepository(AppDBContext appDBContext, IShopCart shopCart) : base(appDBContext)
        {
            _shopCart = shopCart;
        }

        public new async Task<IReadOnlyList<Product>> GetAllAsync()
        {
            return await base.GetAllAsync();
        }

        public new async Task<IReadOnlyList<Product>> GetAllAsync(ISpecification<Product> specification)
        {
            return await base.GetAllAsync(specification);
        }

        public new async Task<Product> GetByIdAsync(int productId)
        {
            return await base.GetByIdAsync(productId);
        }

        public async Task DeleteAsync(int id)
        {
            Product product = await GetByIdAsync(id);
            await DeleteAsync(product);
        }

        public new async Task UpdateAsync(Product product)
        {
            await base.UpdateAsync(product);
        }

        public async Task<Product> GetByNameAsync(string name)
        {
            var collection = await base.GetAllAsync();

            return collection.FirstOrDefault(n => n.Name.Equals(name));
        }

        public async Task<IReadOnlyList<Product>> SearchProductsAsync(string searchText)
        {
            var products = await base.GetAllAsync(new ProductSpecification().SortByRelevance());
            if (searchText != null)
            {
                List<string> searchWords = searchText.Split(" ").ToList();
                foreach (string word in searchWords)
                {
                    products = products.Where(i => i.Name.ToLower().Contains(word.ToLower())).ToList()
                        .Union(products.Where(i => i.Company.ToLower().Contains(word.ToLower()))).Distinct().ToList();
                }
            }

            return products;
        }

        public async Task AddProductAsync(Product product)
        {
            await AddAsync(product);
        }

        public List<FilterCategoryVM> GetFilterCategoriesByProducts(List<Product> products)
        {
            List<FilterCategoryVM> filter = new List<FilterCategoryVM>();
            IEnumerable<string> Categories = products.Select(i => i.Category).Distinct().ToList();
            IEnumerable<string> Countries = products.Select(i => i.Country).Distinct().ToList();
            IEnumerable<string> Companies = products.Select(i => i.Company).Distinct().ToList();
            Dictionary<string, IEnumerable<string>> filterCategory =
                new Dictionary<string, IEnumerable<string>>()
                {
                    { "Категория", Categories },
                    { "Компания", Companies },
                    { "Страна производитель", Countries }
                };
            int categoryId = 1;

            foreach (KeyValuePair<string, IEnumerable<string>> item in filterCategory)
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
                filter.Add(category);
                categoryId++;
            }
            return filter;
        }

        public List<Product> SortProducts(List<Product> products, List<string> filters)
        {
            List<FilterCategoryVM> filterCategories = GetFilterCategoriesByProducts(products);
            int categoryId = 0;
            int filterId = 0;
            List<List<string>> filter = new List<List<string>>() { new List<string>(), new List<string>(), new List<string>() };

            foreach (string item in filters)
            {
                if (item != null)
                {
                    categoryId = Convert.ToInt32(item.Split('-')[0]) - 1;
                    filterId = Convert.ToInt32(item.Split('-')[1]) - 1;
                    string i = filterCategories[categoryId].Selections[filterId].Name;
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

        public async Task<List<ShowProductViewModel>> FindProductsInTheCart(List<Product> products, string email)
        {
            var cartItems = _shopCart.GetAllAsync(
                new ShopCartSpecification().
                WhereUserEmail(email)).GetAwaiter().GetResult().ToList();

            List<ShowProductViewModel> showProducts = new List<ShowProductViewModel>();

            foreach (Product product in products)
            {
                ShowProductViewModel showProduct = new ShowProductViewModel() { Product = product };
                bool itemInCart = false;

                foreach (ShopCartItem shopCart in cartItems)
                {
                    if (shopCart.Product == product)
                    {
                        itemInCart = true;
                        cartItems.Remove(shopCart);
                        break;
                    }
                }
                if (itemInCart)
                {
                    showProduct.IsInCart = true;
                }
                else
                {
                    showProduct.IsInCart = false;
                }
                showProducts.Add(showProduct);
            }

            return showProducts;
        }

        public async Task<ShowProductViewModel> FindProductInTheCart(Product product, string email)
        {
            var cartItems = (await _shopCart.GetAllAsync(
               new ShopCartSpecification().
               WhereUserEmail(email))
               ).ToList();
            ShowProductViewModel showProduct = new ShowProductViewModel() { Product = product };
            bool itemInCart = false;

            foreach (ShopCartItem shopCart in cartItems)
            {
                if (shopCart.Product == product)
                {
                    itemInCart = true;
                    cartItems.Remove(shopCart);
                    break;
                }
            }
            if (itemInCart)
            {
                showProduct.IsInCart = true;
            }
            else
            {
                showProduct.IsInCart = false;
            }

            return showProduct;
        }
        public async Task<List<ShowProductViewModel>> RemoveIfInCart(List<Product> products, string email)
        {
            var cartItems = (await _shopCart.GetAllAsync(
                new ShopCartSpecification().
               WhereUserEmail(email))
               ).ToList();
            List<ShowProductViewModel> showProducts = new List<ShowProductViewModel>();

            foreach (Product product in products)
            {
                ShowProductViewModel showProduct = new ShowProductViewModel() { Product = product };
                bool itemInCart = false;

                foreach (ShopCartItem shopCart in cartItems)
                {
                    if (shopCart.Product == product)
                    {
                        itemInCart = true;
                        cartItems.Remove(shopCart);
                        break;
                    }
                }
                if (!itemInCart)
                {
                    showProduct.IsInCart = false;
                    showProducts.Add(showProduct);
                }
            }

            return showProducts;
        }
    }
}
