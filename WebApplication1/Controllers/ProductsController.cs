using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controlles
{
    public class ProductsController : Controller
    {
        private readonly IAllProduct _allProducts;
        private readonly IProductsCategory _allCategory;
        private readonly ShopCart _shopCart;
        private readonly string[] _filters;
        private string[] _filter;


        public ProductsController(IAllProduct iAllProducts, IProductsCategory iProductsCat, ShopCart shopCart)
        {
            _allProducts = iAllProducts;
            _allCategory = iProductsCat;
            _shopCart = shopCart;
            _filters = _allCategory.AllCategories.Select(p => p.CategoryName).ToArray();
        }

        [Route("Products/List")]
        [Route("Products/List/{category}")]
        public ViewResult List(string filter)
        {
            _filter = filter?.Split('_');
            IEnumerable<Product> products = null;
            if (filter == null)
            {
                products = _allProducts.Products.OrderBy(i => i.Id);
            }
            else
            {
                products = _allProducts.Products.Where(i => i.Category.CategoryName == _filter.FirstOrDefault()).OrderBy(i => i.Id);
                foreach (string item in _filter)
                {
                    if (item == _filter.FirstOrDefault()) continue;
                    products = products.Select(product => product).Concat(_allProducts.Products.Where(i => i.Category.CategoryName.Equals(item)).OrderBy(i => i.Id));
                }
            }
            var carObj = new ProductsListViewModel
            {
                AllProducts = products,
                ShopCart = _shopCart,
                FilterSort = _filters,
                Filter = _filter
            };

            ViewBag.Title = "Страница с товарами";

            return View(carObj);
        }

        public RedirectToActionResult AddToCart(int IdProduct, string filter)
        {
            var item = _allProducts.Products.FirstOrDefault(i => i.Id == IdProduct);
            if (item != null)
            {
                _shopCart.AddToCart(item);
            }

            return RedirectToAction("List", new { filter });
        }

        public RedirectToActionResult RemoveToCart(string IdProduct, string filter)
        {
            _shopCart.RemoveToCart(IdProduct);

            return RedirectToAction("List", new { filter });
        }
    }
}
