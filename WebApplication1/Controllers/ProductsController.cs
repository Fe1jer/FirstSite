using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controlles
{
    public class ProductsController : Controller
    {
        private readonly IAllProduct _allProducts;
        private readonly IProductFilter _filter;
        private readonly ShopCart _shopCart;
        private ProductFilter _userFilter;


        public ProductsController(IAllProduct iAllProducts, IProductFilter filter, ShopCart shopCart)
        {
            _allProducts = iAllProducts;
            _shopCart = shopCart;
            _filter = filter;
        }

        [Route("Products/List")]
        public ViewResult List(string category, string company, string country)
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
            var carObj = new ProductsListViewModel
            {
                AllProducts = products,
                ShopCart = _shopCart,
                FilterSort = _filter,
                Filter = new ProductFilter { AllCategory= category, AllCompany= company, AllCountry= country }
            };

            ViewBag.Title = "Страница с товарами";

            return View(carObj);
        }

        public RedirectToActionResult AddToCart(int IdProduct, string category, string company, string country)
        {
            var item = _allProducts.Products.FirstOrDefault(i => i.Id == IdProduct);
            if (item != null)
            {
                _shopCart.AddToCart(item);
            }

            return RedirectToAction("List", new { category, company, country });
        }

        public RedirectToActionResult RemoveToCart(string IdProduct, string category, string company, string country)
        {
            _shopCart.RemoveToCart(IdProduct);

            return RedirectToAction("List", new { category, company, country });
        }
    }
}
