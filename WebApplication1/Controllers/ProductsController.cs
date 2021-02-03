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

        public IActionResult ProductNotFound()
        {
            return View();
        }

        [Route("Products/ChangeProduct")]
        public IActionResult ChangeProduct(int id)
        {
            Product obj = _allProducts.Products.FirstOrDefault(p => p.Id == id);
            ViewBag.Title = "Изменение товара";

            return View(obj);
        }

        [Route("Products/ChangeProduct")]
        [HttpPost]
        public IActionResult ChangeProduct(Product obj)
        {
            ViewBag.Title = "Изменение товара";
            if (ModelState.IsValid)
            {
                _allProducts.ChangeProduct(obj);
                return RedirectToAction("Index");
            }

            return View(obj);
        }

        [Route("Products/DeleteProduct")]
        public IActionResult DeleteProduct(int id)
        {
            _allProducts.DeleteProduct(id);
            return RedirectToAction("Index");
        }

        [Route("Products/AddProduct")]
        public IActionResult AddProduct()
        {
            ViewBag.Title = "Добавление товара";

            return View();
        }

        [Route("Products/AddProduct")]
        [HttpPost]
        public IActionResult AddProduct(Product obj)
        {
            if (ModelState.IsValid)
            {
                _allProducts.CreateProduct(obj);
                return RedirectToAction("Index");
            }

            return View(obj);
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

            IEnumerable<Product> products = _allProducts.Products.OrderByDescending(i => i.IsFavourite);

            if (!_userFilter.IsEmpty())
            {
                string[] filter;
                if (_userFilter.AllCategory != null)
                {
                    filter = _userFilter.AllCategory.Split('_');
                    products = products.Where(i => filter.Contains(i.Category)).OrderByDescending(i => i.IsFavourite).ToList();
                }
                if (_userFilter.AllCompany != null)
                {
                    filter = _userFilter.AllCompany.Split('_');
                    products = products.Where(i => filter.Contains(i.Company)).OrderByDescending(i => i.IsFavourite).ToList();
                }
                if (_userFilter.AllCountry != null)
                {
                    filter = _userFilter.AllCountry.Split('_');
                    products = products.Where(i => filter.Contains(i.Country)).OrderByDescending(i => i.IsFavourite).ToList();
                }
            }
            var productObj = new ProductsListViewModel
            {
                AllProducts = products,
                ShopCart = _shopCart,
                FilterSort = _filter,
                Filter = new ProductFilter { AllCategory = category, AllCompany = company, AllCountry = country }
            };

            ViewBag.Title = "Все товары";

            return View(productObj);
        }

        [Route("Products/AddToCart")]
        public RedirectToActionResult AddToCart(int IdProduct, string category, string company, string country)
        {
            Product item = _allProducts.Products.FirstOrDefault(i => i.Id == IdProduct);
            if (item != null)
            {
                _shopCart.AddToCart(item);
            }

            return RedirectToAction("Index", new { category, company, country });
        }

        [Route("Products/RemoveToCart")]
        public RedirectToActionResult RemoveToCart(string IdProduct, string category, string company, string country)
        {
            _shopCart.RemoveToCart(IdProduct);

            return RedirectToAction("Index", new { category, company, country });
        }
    }
}
