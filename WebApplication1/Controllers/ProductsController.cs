using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controlles
{
    public class ListController : Controller
    {
        private readonly IAllProduct _allProducts;
        private readonly IProductFilter _filter;
        private readonly ShopCart _shopCart;
        private ProductFilter _userFilter;


        public ListController(IAllProduct iAllProducts, IProductFilter filter, ShopCart shopCart)
        {
            _allProducts = iAllProducts;
            _shopCart = shopCart;
            _filter = filter;
        }

        public IActionResult ProductNotFound()
        {
            return View();
        }
        
        [Route("Products/{name}")]
        public IActionResult Product(string name)
        {
            Product obj = _allProducts.Products.Where(i => i.Name == name.Replace("-", " ")).FirstOrDefault();
            if (obj != null)
            {
                ViewBag.Title = obj.Company + " " + obj.Name;
                string i = _shopCart.GetShopItems().Where(o => o.Product.Name == name.Replace("-", " ")).FirstOrDefault()?.ShopCartId;
                ProductPage product = new ProductPage { Product = obj, ShopCartId = i };
                return View(product);
            }
            else
            {
                return RedirectToAction("ProductNotFound");
            }
        }
        
        [Route("Products")]
        public IActionResult Index(string category, string company, string country)
        {
            _userFilter = new ProductFilter { AllCategory = category, AllCompany = company, AllCountry = country };

            IEnumerable<Product> products = _allProducts.Products.OrderBy(i => i.IsFavourite);

            if (!_userFilter.IsEmpty())
            {
                string[] filter;
                if (_userFilter.AllCategory != null)
                {
                    filter = _userFilter.AllCategory.Split('_');
                    products = products.Where(i => filter.Contains(i.Category)).OrderBy(i => i.IsFavourite).ToList();
                }
                if (_userFilter.AllCompany != null)
                {
                    filter = _userFilter.AllCompany.Split('_');
                    products = products.Where(i => filter.Contains(i.Company)).OrderBy(i => i.IsFavourite).ToList();
                }
                if (_userFilter.AllCountry != null)
                {
                    filter = _userFilter.AllCountry.Split('_');
                    products = products.Where(i => filter.Contains(i.Country)).OrderBy(i => i.IsFavourite).ToList();
                }
            }
            var productObj = new ProductsListViewModel
            {
                AllProducts = products,
                ShopCart = _shopCart,
                FilterSort = _filter,
                Filter = new ProductFilter { AllCategory= category, AllCompany= company, AllCountry= country }
            };

            ViewBag.Title = "Все товары";

            return View(productObj);
        }

        public RedirectToActionResult AddToCart(int IdProduct, string category, string company, string country)
        {
            Product item = _allProducts.Products.FirstOrDefault(i => i.Id == IdProduct);
            if (item != null)
            {
                _shopCart.AddToCart(item);
            }

            return RedirectToAction("Index", new { category, company, country });
        }

        public RedirectToActionResult RemoveToCart(string IdProduct, string category, string company, string country)
        {
            _shopCart.RemoveToCart(IdProduct);

            return RedirectToAction("Index", new { category, company, country });
        }
    }
}
