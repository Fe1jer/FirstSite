using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Models;
using WebApplication1.Data.Specifications;
using WebApplication1.ViewModels;

namespace WebApplication1.Controlles
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly IShopCart _shopCart;

        public ProductsController(IShopCart shopCart, IProductRepository IProductRepository, IUserRepository IUserRepository)
        {
            _shopCart = shopCart;
            _userRepository = IUserRepository;
            _productRepository = IProductRepository;
        }

        [HttpGet, Route("Catalog/Search")]
        public async Task<IActionResult> Search(string q, List<string> filters)
        {
            var products = await _productRepository.SearchProductsAsync(q);
            products = products.OrderByDescending(p => p.Available).ThenByDescending(p => p.IsFavourite).ThenByDescending(p => p.Id).ToList();
            List<FilterCategoryVM> filterCategories = _productRepository.GetFilterCategoriesByProducts(products.ToList());
            products = _productRepository.SortProducts(products.ToList(), filters);
            List<ShowProductViewModel> showProducts = _productRepository.FindProductsInTheCart(products.ToList(), (List<ShopCartItem>)await _shopCart.GetShopItemsAsync(new ShopCartSpecification().IncludeProduct().WhereUser(await _userRepository.GetUserAsync(User.Identity.Name))));

            var productObj = new ProductsListViewModel
            {
                AllProducts = showProducts,
                FilterSort = filterCategories,
                Filter = filters
            };
            var model = new SearchViewModel
            {
                SearchName = q,
                ProductsListViewModel = productObj
            };

            return View(model);
        }

        [Route("Products/ChangeProduct"), Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> ChangeProduct(int id)
        {
            User user = await _userRepository.GetUserAsync(User.Identity.Name);
            if (user.Role.Name != "admin" && user.Role.Name != "moderator")
            {
                return RedirectToAction("Logout", "Account");
            }

            Product obj = await _productRepository.GetProductByIdAsync(id);

            return View(obj);
        }

        [HttpPost, Route("Products/ChangeProduct"), Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> ChangeProduct(Product obj)
        {
            if (ModelState.IsValid)
            {
                await _productRepository.UpdateProductAsync(obj);

                return RedirectToAction("Index");
            }

            return View(obj);
        }

        [Route("Products/DeleteProduct"), Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            Product product = await _productRepository.GetProductByIdAsync(id);
            await _productRepository.DeleteProductAsync(product);

            return RedirectToAction("Index");
        }

        [Route("Products/AddProduct"), Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> AddProduct()
        {
            User user = await _userRepository.GetUserAsync(User.Identity.Name);
            if (user.Role.Name != "admin" && user.Role.Name != "moderator")
            {
                return RedirectToAction("Logout", "Account");
            }

            return View();
        }

        [HttpPost, Route("Products/AddProduct"), Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> AddProduct(Product obj)
        {
            if (await _productRepository.GetProductByNameAsync(obj.Name) == null)
            {
                if (ModelState.IsValid)
                {
                    await _productRepository.AddProductAsync(obj);

                    return RedirectToAction("Index");
                }

                return View(obj);
            }
            else
            {
                ModelState.AddModelError("", "Товар имеется в базе данных");

                return View(obj);
            }
        }

        [Route("Products/{name}")]
        public async Task<IActionResult> Product(int id)
        {
            int i = 0;
            Product obj = await _productRepository.GetProductByIdAsync(id);
            if (obj != null)
            {
                ShopCartItem item = _shopCart.GetShopItemsAsync(new ShopCartSpecification()
                    .IncludeProduct()
                    .WhereUser(await _userRepository.GetUserAsync(User.Identity.Name))).Result
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
                return NotFound();
            }
        }

        [Route("Catalog")]
        public async Task<IActionResult> Index(List<string> filters)
        {
            IEnumerable<int> vs = new List<int>() { 1,2,3,4,5};
            vs.Where(p => p > 0);
            foreach(int item in vs)
            {
                int a = item;
            }

            var products = await _productRepository.GetProductsAsync();
            products = products.OrderByDescending(p=>p.Available).ThenByDescending(p => p.IsFavourite).ThenByDescending(p => p.Id).ToList();
            List<FilterCategoryVM> filterCategories = _productRepository.GetFilterCategoriesByProducts(products.ToList());
            products = _productRepository.SortProducts(products.ToList(), filters);
            List<ShowProductViewModel> showProducts = _productRepository.FindProductsInTheCart(products.ToList(), (List<ShopCartItem>)await _shopCart.GetShopItemsAsync(new ShopCartSpecification().IncludeProduct().WhereUser(await _userRepository.GetUserAsync(User.Identity.Name))));

            var productObj = new ProductsListViewModel
            {
                AllProducts = showProducts,
                FilterSort = filterCategories,
                Filter = filters
            };

            return View(productObj);
        }

        [HttpPost]
        [Route("Catalog/IndexAjax")]
        public async Task<IActionResult> IndexAjax(List<string> filters)
        {
            var products = await _productRepository.GetProductsAsync();
            products = products.OrderByDescending(p => p.Available).ThenByDescending(p => p.IsFavourite).ThenByDescending(p => p.Id).ToList();
            products = _productRepository.SortProducts(products.ToList(), filters);
            List<ShowProductViewModel> showProducts = _productRepository.FindProductsInTheCart(products.ToList(), (List<ShopCartItem>)await _shopCart.GetShopItemsAsync(new ShopCartSpecification().IncludeProduct().WhereUser(await _userRepository.GetUserAsync(User.Identity.Name))));
            Thread.Sleep(1000);

            return Json(showProducts);
        }

        [HttpPost]
        [Route("Catalog/SearchAjax")]
        public async Task<IActionResult> SearchAjax(string q, List<string> filters)
        {
            var products = await _productRepository.SearchProductsAsync(q);
            products = products.OrderByDescending(p => p.Available).ThenByDescending(p => p.IsFavourite).ThenByDescending(p => p.Id).ToList();
            products = _productRepository.SortProducts(products.ToList(), filters);
            List<ShowProductViewModel> showProducts = _productRepository.FindProductsInTheCart(products.ToList(), (List<ShopCartItem>)await _shopCart.GetShopItemsAsync(new ShopCartSpecification().IncludeProduct().WhereUser(await _userRepository.GetUserAsync(User.Identity.Name))));
            Thread.Sleep(1000);

            return Json(showProducts);
        }
    }
}
