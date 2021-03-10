using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Models;
using WebApplication1.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication1.Data.Specifications;

namespace WebApplication1.Controlles
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _allProducts;
        private readonly IUserRepository _allUser;
        private readonly IProductFilter _filter;
        private readonly IShopCart _shopCart;

        public ProductsController(IShopCart shopCart, IProductRepository iAllProducts, IProductFilter filter, IUserRepository allUser)
        {
            _shopCart = shopCart;
            _allUser = allUser;
            _allProducts = iAllProducts;
            _filter = filter;
        }

        [Route("Products/ProductNotFound")]
        public IActionResult ProductNotFound()
        {
            return View();
        }

        [Authorize(Roles = "admin, moderator")]
        [Route("Products/ChangeProduct")]
        public async Task<IActionResult> ChangeProduct(int id) {
            User user = await _allUser.GetUserAsync(User.Identity.Name);
            if (user.Role.Name!="admin" && user.Role.Name!= "moderator")
            {
                return RedirectToAction("Logout", "Account");
            }

            Product obj = await _allProducts.GetProductByIdAsync(id);
            ViewBag.Title = "Изменение товара";

            return View(obj);
        }

        [Authorize(Roles = "admin, moderator")]
        [Route("Products/ChangeProduct")]
        [HttpPost]
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

        [Authorize(Roles = "admin, moderator")]
        [Route("Products/DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            Product product = await _allProducts.GetProductByIdAsync(id);
            await _allProducts.DeleteProductAsync(product);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin, moderator")]
        [Route("Products/AddProduct")]
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

        [Authorize(Roles = "admin, moderator")]
        [Route("Products/AddProduct")]
        [HttpPost]
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

        [Route("Products")]
        public async Task<IActionResult> Index(string category, string company, string country)
        {
            ProductFilter _userFilter = new ProductFilter { AllCategory = category, AllCompany = company, AllCountry = country };

            var products = await _allProducts.GetProductsAsync(new ProductSpecification().SortByFavourite());

            if (!_userFilter.IsEmpty())
            {
                string[] filter;
                if (_userFilter.AllCategory != null)
                {
                    filter = _userFilter.AllCategory.Split('_');
                    products = products.Where(i => filter.Contains(i.Category)).ToList();
                }
                if (_userFilter.AllCompany != null)
                {
                    filter = _userFilter.AllCompany.Split('_');
                    products = products.Where(i => filter.Contains(i.Company)).ToList();
                }
                if (_userFilter.AllCountry != null)
                {
                    filter = _userFilter.AllCountry.Split('_');
                    products = products.Where(i => filter.Contains(i.Country)).ToList();
                }
            }
            var productObj = new ProductsListViewModel
            {
                AllProducts = products,
                ShopCartItems = (List<ShopCartItem>)await _shopCart.GetShopItemsAsync(new ShopCartSpecification().IncludeProduct().WhereUser(await _allUser.GetUserAsync(User.Identity.Name))),
                FilterSort = _filter,
                Filter = new ProductFilter { AllCategory = category, AllCompany = company, AllCountry = country }
            };

            ViewBag.Title = "Все товары";

            return View(productObj);
        }

        [Authorize]
        [Route("Products/AddToCart")]
        public async Task<RedirectToActionResult> AddToCart(int IdProduct, string category, string company, string country)
        {
            Product item = await _allProducts.GetProductByIdAsync(IdProduct);
            if (item != null)
            {
                await _shopCart.AddToCart(await _allUser.GetUserAsync(User.Identity.Name), item);
            }

            return RedirectToAction("Index", new { category, company, country });
        }

        [Authorize]
        [Route("Products/RemoveToCart")]
        public async Task<RedirectToActionResult> RemoveToCart(int IdProduct, string category, string company, string country)
        {
            await _shopCart.RemoveToCart(await _allUser.GetUserAsync(User.Identity.Name), IdProduct);

            return RedirectToAction("Index", new { category, company, country });
        }
    }
}
