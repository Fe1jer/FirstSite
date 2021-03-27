using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Models;
using WebApplication1.Data.Specifications;
using WebApplication1.ViewModels;

namespace WebApplication1.Controlles
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _allProducts;
        private readonly IProductFilter _productFilter;
        private readonly IUserRepository _allUser;
        private readonly IShopCart _shopCart;

        public ProductsController(IShopCart shopCart, IProductRepository iAllProducts, IUserRepository allUser, IProductFilter productFilter)
        {
            _productFilter = productFilter;
            _shopCart = shopCart;
            _allUser = allUser;
            _allProducts = iAllProducts;
        }

        [Route("Products/ProductNotFound")]
        public IActionResult ProductNotFound()
        {
            return View();
        }

        [Route("Products/ChangeProduct"), Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> ChangeProduct(int id)
        {
            User user = await _allUser.GetUserAsync(User.Identity.Name);
            if (user.Role.Name != "admin" && user.Role.Name != "moderator")
            {
                return RedirectToAction("Logout", "Account");
            }

            Product obj = await _allProducts.GetProductByIdAsync(id);
            ViewBag.Title = "Изменение товара";

            return View(obj);
        }

        [HttpPost, Route("Products/ChangeProduct"), Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> ChangeProduct(Product obj)
        {
            ViewBag.Title = "Изменение товара";
            if (ModelState.IsValid)
            {
                await _allProducts.UpdateProductAsync(obj);
                return RedirectToAction("Index");
            }

            return View(obj);
        }

        [Route("Products/DeleteProduct"), Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            Product product = await _allProducts.GetProductByIdAsync(id);
            await _allProducts.DeleteProductAsync(product);
            return RedirectToAction("Index");
        }

        [Route("Products/AddProduct"), Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> AddProduct()
        {
            User user = await _allUser.GetUserAsync(User.Identity.Name);
            if (user.Role.Name != "admin" && user.Role.Name != "moderator")
            {
                return RedirectToAction("Logout", "Account");
            }

            ViewBag.Title = "Добавление товара";

            return View();
        }

        [HttpPost, Route("Products/AddProduct"), Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> AddProduct(Product obj)
        {
            ViewBag.Title = "Добавление товара";
            if (await _allProducts.GetProductByNameAsync(obj.Name) == null)
            {
                if (ModelState.IsValid)
                {
                    await _allProducts.AddProductAsync(obj);
                    return RedirectToAction("Index");
                }
                return View(obj);
            }
            else
            {
                ModelState.AddModelError("", "товар имеется в базе данных");
                return View(obj);
            }
        }

        [Route("Products/{name}")]
        public async Task<IActionResult> Product(string name, int id)
        {
            int i = 0;
            Product obj = await _allProducts.GetProductByIdAsync(id);
            if (obj != null)
            {
                ViewBag.Title = obj.Company + " " + obj.Name;
                ShopCartItem item = _shopCart.GetShopItemsAsync(new ShopCartSpecification().IncludeProduct().WhereUser(await _allUser.GetUserAsync(User.Identity.Name))).Result.Where(o => o.Product.Id == id).FirstOrDefault() ?? null;
                if (item != null)
                {
                    i = item.Product.Id;
                }
                ProductPage product = new ProductPage { Product = obj, ShopCartItemId = i };
                return View(product);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet, Route("Products")]
        public async Task<IActionResult> Index(Dictionary<string, int> filters)
        {
            int categoryId = 0;
            ProductFilter _userFilter = new ProductFilter { AllCategory = null, AllCompany = null, AllCountry = null };

            List<List<string>> filter = new List<List<string>>() { new List<string>(), new List<string>(), new List<string>() };
            var products = await _allProducts.GetProductsAsync(new ProductSpecification().SortByFavourite());
            foreach (KeyValuePair<string, int> item in filters)
            {
                if (item.Value != 0)
                {
                    categoryId = Convert.ToInt32(item.Key.Split('-')[0]) - 1;
                    int filterId = item.Value - 1;
                    string i = _productFilter.FilterCategories[categoryId].Selections[filterId].Name;
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
            var productObj = new ProductsListViewModel
            {
                AllProducts = products,
                ShopCartItems = (List<ShopCartItem>)await _shopCart.GetShopItemsAsync(new ShopCartSpecification().IncludeProduct().WhereUser(await _allUser.GetUserAsync(User.Identity.Name))),
                FilterSort = _productFilter,
                Filter = filters
            };

            ViewBag.Title = "Все товары";

            return View(productObj);
        }

        [Route("Products/AddToCart"), Authorize]
        public async Task<RedirectToActionResult> AddToCart(int IdProduct, Dictionary<string, int> filters)
        {
            Product item = await _allProducts.GetProductByIdAsync(IdProduct);
            if (item != null)
            {
                await _shopCart.AddToCart(await _allUser.GetUserAsync(User.Identity.Name), item);
            }

            return RedirectToAction("Index", filters);
        }

        [Route("Products/RemoveToCart"), Authorize]
        public async Task<RedirectToActionResult> RemoveToCart(int IdProduct, Dictionary<string, int> filters)
        {
            await _shopCart.RemoveToCart(await _allUser.GetUserAsync(User.Identity.Name), IdProduct);

            return RedirectToAction("Index", filters);
        }
    }
}
