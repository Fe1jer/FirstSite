using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        [HttpGet, Route("Products/Search")]
        public async Task<IActionResult> Search(string search)
        {
            var products = await _allProducts.GetProductsAsync();
            products = products.Where(i => i.Name.ToLower().Contains(search.ToLower())).ToList()
                .Union(products.Where(i => i.Company.ToLower().Contains(search.ToLower()))).Distinct().ToList();
            var productObj = new ProductsListViewModel
            {
                AllProducts = products,
                ShopCartItems = (List<ShopCartItem>)await _shopCart.GetShopItemsAsync(new ShopCartSpecification().IncludeProduct().WhereUser(await _allUser.GetUserAsync(User.Identity.Name))),
                FilterSort = _productFilter.GetFilterCategoriesByProducts(products.ToList())
            };
            var model = new SearchViewModel
            {
                SearchName = search,
                ProductsListViewModel = productObj
            };
            return View(model);
        }

        [HttpPost, Route("Products/Search")]
        public async Task<IActionResult> Search(Dictionary<string, int> filters, string search)
        {
            var products = await _allProducts.SearchProductsAsync(search);
            List<FilterCategoryVM> filterCategories = _productFilter.GetFilterCategoriesByProducts(products.ToList());
            products = _productFilter.SortProducts(products.ToList(), filters);
            var productObj = new ProductsListViewModel
            {
                AllProducts = products,
                ShopCartItems = (List<ShopCartItem>)await _shopCart.GetShopItemsAsync(new ShopCartSpecification().IncludeProduct().WhereUser(await _allUser.GetUserAsync(User.Identity.Name))),
                FilterSort = filterCategories,
                Filter = filters
            };
            var model = new SearchViewModel
            {
                SearchName = search,
                ProductsListViewModel = productObj
            };
            return View(model);
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

            return View(obj);
        }

        [HttpPost, Route("Products/ChangeProduct"), Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> ChangeProduct(Product obj)
        {
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

            return View();
        }

        [HttpPost, Route("Products/AddProduct"), Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> AddProduct(Product obj)
        {
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
        public async Task<IActionResult> Product(int id)
        {
            int i = 0;
            Product obj = await _allProducts.GetProductByIdAsync(id);
            if (obj != null)
            {
                ShopCartItem item = _shopCart.GetShopItemsAsync(new ShopCartSpecification()
                    .IncludeProduct()
                    .WhereUser(await _allUser.GetUserAsync(User.Identity.Name))).Result
                    .Where(o => o.Product.Id == id).FirstOrDefault() ?? null;
                if (item != null)
                {
                    i = item.Product.Id;
                }
                ProductPage product = new ProductPage { Product = obj, ShopCartItemId = i };
                return View(product);
            }
            else
            {
                return View("ProductNotFound");
            }
        }

        [Route("Products")]
        public async Task<IActionResult> Index(Dictionary<string, int> filters, string deleteFilter)
        {
            if (deleteFilter != null)
            {
                filters.Remove(deleteFilter.Split('-')[0]);
            }
            var products = await _allProducts.GetProductsAsync(new ProductSpecification().SortByFavourite());
            List<FilterCategoryVM> filterCategories = _productFilter.GetFilterCategoriesByProducts(products.ToList());
            products = _productFilter.SortProducts(products.ToList(), filters);
            var productObj = new ProductsListViewModel
            {
                AllProducts = products,
                ShopCartItems = (List<ShopCartItem>)await _shopCart.GetShopItemsAsync(new ShopCartSpecification().IncludeProduct().WhereUser(await _allUser.GetUserAsync(User.Identity.Name))),
                FilterSort = filterCategories,
                Filter = filters
            };

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
