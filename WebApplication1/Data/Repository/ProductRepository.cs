using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetShop.Data.Interfaces;
using InternetShop.Data.Models;
using InternetShop.Data.Specifications;
using InternetShop.Data.Specifications.Base;
using InternetShop.ViewModels;
using InternetShop.Data.Repository.Base;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace InternetShop.Data.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IShopCart _shopCart;
        private readonly IAttributeRepository _attributeCategory;
        private readonly IAttributeValueRepository _productAttribute;
        private readonly IProductImagesRepository _productImagesRepository;
        private readonly IProductTypesRepository _productTypes;

        public ProductRepository(AppDBContext appDBContext, IShopCart shopCart, IAttributeRepository attributeCategory, IAttributeValueRepository productAttribute, IProductTypesRepository productTypes, IWebHostEnvironment appEnvironment, IProductImagesRepository productImagesRepository) : base(appDBContext)
        {
            _productImagesRepository = productImagesRepository;
            _productAttribute = productAttribute;
            _attributeCategory = attributeCategory;
            _shopCart = shopCart;
            _productTypes = productTypes;
            _appEnvironment = appEnvironment;
        }

        public new async Task<IReadOnlyList<Product>> GetAllAsync()
        {
            return await base.GetAllAsync(new ProductSpecification());
        }

        public new async Task<IReadOnlyList<Product>> GetAllAsync(ISpecification<Product> specification)
        {
            return await base.GetAllAsync(specification);
        }

        public new async Task<Product> GetByIdAsync(int productId)
        {
            var products = await GetAllAsync();
            return products.FirstOrDefault(u => u.Id == productId);
        }

        public async Task DeleteAsync(int id)
        {
            Product product = await GetByIdAsync(id);
            await DeleteAsync(product);
        }

        private async Task<Product> UpdateProductImages(CreateChangeProductViewModel model)
        {
            if (model.Uploads != null && model.Uploads.Count != 0)
            {
                if (model.Product.ProductImages != null)
                {
                    foreach (var img in model.Product?.ProductImages)
                    {
                        if (File.Exists($"wwwroot{img}"))
                        {
                            File.Delete($"wwwroot{img}");
                        }
                    }
                }
                await _productImagesRepository.DeleteListAsync(model.Product.ProductImages);
                model.Product.ProductImages = new List<ProductImage>();
                foreach (var upload in model.Uploads)
                {
                    string path = "\\img\\Products\\" + model.Product.Id;
                    // сохраняем файл в папку Files в каталоге wwwroot

                    Directory.GetCurrentDirectory();
                    var a = $"{_appEnvironment.WebRootPath + path}";
                    Directory.CreateDirectory(a);
                    path += "\\" + upload.FileName;

                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await upload.CopyToAsync(fileStream);
                    }

                    ProductImage image = new ProductImage()
                    {
                        pathImg = path
                    };
                    model.Product.ProductImages.Add(image);
                }
                model.Product.Img = model.Product.ProductImages.FirstOrDefault().pathImg;
            }

            return model.Product;
        }

        public async Task UpdateAsync(CreateChangeProductViewModel model)
        {
            var type = await _productTypes.FindByType(model.Product.ProductType);
            if (type != null)
            {
                model.Product.ProductType = type;
            }
            var productAttributes = await _productAttribute.GetAllAsync(new AttributeValuesSpecification().WhereProductId(model.Product.Id));
            List<AttributeValue> notProductAttributes;

            if (model.Product.AttributeValues != null)
            {
                var attributeCategories = await _attributeCategory.GetAllAsync();

                for (int i = 0; i < model.Product.AttributeValues.Count; i++)
                {
                    var productAttribute = productAttributes.FirstOrDefault(a => a.Id == model.Product.AttributeValues[i]?.Id);
                    var attributeCategory = attributeCategories.FirstOrDefault(a => a.Name == model.Product.AttributeValues[i].Attribute.Name);

                    if (productAttribute != null)
                    {
                        productAttribute.Value = model.Product.AttributeValues[i].Value;
                        model.Product.AttributeValues[i] = productAttribute;
                    }
                    if (attributeCategory != null)
                    {
                        model.Product.AttributeValues[i].Attribute = attributeCategory;
                    }
                }
                notProductAttributes = productAttributes.Where(p => !model.Product.AttributeValues.Select(p => p?.Id).Contains(p?.Id)).ToList();
            }
            else
            {
                notProductAttributes = productAttributes.ToList();
            }
            await _productAttribute.DeleteListAsync(notProductAttributes);
            model.Product = await UpdateProductImages(model);

            await UpdateAsync(model.Product);
        }

        public async Task<Product> GetByNameAsync(string name)
        {
            var collection = await GetAllAsync();

            return collection.FirstOrDefault(n => n.ProductType.Name.Equals(name));
        }

        public async Task<IReadOnlyList<Product>> SearchProductsAsync(string searchText)
        {
            var products = await GetAllAsync(new ProductSpecification().SortByRelevance());
            if (searchText != null)
            {
                List<string> searchWords = searchText.Split(" ").ToList();
                foreach (string word in searchWords)
                {
                    products = products.Where(i => i.ProductType.Name.ToLower().Contains(word.ToLower())).ToList()
                        .Union(products.Where(i => i.ProductType.Company.ToLower().Contains(word.ToLower()))).Distinct().ToList();
                }
            }

            return products;
        }

        public async Task AddProductAsync(CreateChangeProductViewModel model)
        {
            var type = await _productTypes.FindByType(model.Product.ProductType);
            if (type != null)
            {
                model.Product.ProductType = type;
            }
            if (model.Product.AttributeValues != null)
            {
                var attributeCategories = await _attributeCategory.GetAllAsync();
                foreach (var attribute in model.Product.AttributeValues)
                {
                    var attributeCategory = attributeCategories.FirstOrDefault(a => a.Name == attribute.Attribute.Name);
                    if (attributeCategory != null)
                    {
                        attribute.Attribute = attributeCategory;
                    }
                }
            }

            model.Product = await UpdateProductImages(model);

            await AddAsync(model.Product);
        }

        public List<FilterCategoryVM> GetFilterCategoriesByProducts(List<Product> products)
        {
            List<FilterCategoryVM> filter = new List<FilterCategoryVM>();
            IEnumerable<string> Categories = products.Select(i => i.ProductType.Category).Distinct().ToList();
            IEnumerable<string> Countries = products.Select(i => i.Country).Distinct().ToList();
            IEnumerable<string> Companies = products.Select(i => i.ProductType.Company).Distinct().ToList();
            Dictionary<string, IEnumerable<string>> filterCategory =
                new Dictionary<string, IEnumerable<string>>()
                {
                    { "Категория", Categories },
                    { "Компания", Companies },
                    { "Страна производитель", Countries }
                };
            int categoryId = 0;

            foreach (KeyValuePair<string, IEnumerable<string>> item in filterCategory)
            {
                FilterCategoryVM category = new FilterCategoryVM
                {
                    ID = categoryId,
                    Name = item.Key,
                    Selections = new List<FilterSelectionVM>()
                };
                int selectionId = 0;

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
                    categoryId = Convert.ToInt32(item.Split('-')[0]);
                    filterId = Convert.ToInt32(item.Split('-')[1]);
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
                        products = products.Where(i => item.Contains(i.ProductType.Category)).ToList();
                    }
                    else if (categoryId == 1)
                    {
                        products = products.Where(i => item.Contains(i.ProductType.Company)).ToList();
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
            List<ShowProductViewModel> showProducts = new List<ShowProductViewModel>();

            foreach (Product product in products)
            {
                showProducts.Add(await FindProductInTheCart(product, email));
            }

            return showProducts.OrderByDescending(p => p.IsAvailable).ToList();
        }

        public async Task<ShowProductViewModel> FindProductInTheCart(Product product, string email)
        {
            var cartItems = (await _shopCart.GetAllAsync(
               new ShopCartSpecification().WhereUserEmail(email))).ToList();
            ShowProductViewModel showProduct = new ShowProductViewModel() { Product = product };

            showProduct.IsInCart = IsProductInTheCart(ref cartItems, product);
            showProduct.IsAvailable = product.Count > 0;

            return showProduct;
        }

        public bool IsProductInTheCart(ref List<ShopCartItem> cartItems, Product product)
        {
            foreach (ShopCartItem shopCart in cartItems)
            {
                if (shopCart.Product == product)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<List<ShowProductViewModel>> RemoveIfInCart(List<Product> products, string email)
        {
            var cartItems = (await _shopCart.GetAllAsync(
                new ShopCartSpecification().WhereUserEmail(email))).ToList();
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

        public async Task BuyGoods(List<Product> products)
        {
            foreach (Product product in products)
            {
                product.Count -= 1;
                await UpdateAsync(product);
            }
        }
    }
}
