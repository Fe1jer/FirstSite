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

        public ProductsController(IProductRepository IProductRepository, IUserRepository IUserRepository)
        {
            _userRepository = IUserRepository;
            _productRepository = IProductRepository;
        }

        [HttpGet, Route("Catalog/Search")]
        public async Task<IActionResult> Search(string q, List<string> filters)
        {
            var products = await _productRepository.SearchProductsAsync(q);
            List<FilterCategoryVM> filterCategories = _productRepository.GetFilterCategoriesByProducts(products.ToList());
            products = _productRepository.SortProducts(products.ToList(), filters);
            List<ShowProductViewModel> showProducts = await _productRepository.FindProductsInTheCart(products.ToList(), User.Identity.Name);

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

            Product obj = await _productRepository.GetByIdAsync(id);

            return View(obj);
        }

        [HttpPost, ValidateAntiForgeryToken, Route("Products/ChangeProduct")]
        public async Task<IActionResult> ChangeProduct(Product obj)
        {
            if (ModelState.IsValid)
            {
                await _productRepository.UpdateAsync(obj);

                return RedirectToAction("Index");
            }

            return View(obj);
        }

        [Route("Products/DeleteProduct"), Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productRepository.DeleteAsync(id);

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

        [HttpPost, ValidateAntiForgeryToken, Route("Products/AddProduct")]
        public async Task<IActionResult> AddProduct(Product obj)
        {
            if (await _productRepository.GetByNameAsync(obj.Name) == null)
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
            Product obj = await _productRepository.GetByIdAsync(id);

            if (obj != null)
            {
                ShowProductViewModel showProducts = await _productRepository.FindProductInTheCart(obj, User.Identity.Name);

                return View(showProducts);
            }
            else
            {
                return NotFound();
            }
        }

        [Route("Catalog")]
        public async Task<IActionResult> Index(List<string> filters)
        {
            var products = await _productRepository.GetAllAsync(new ProductSpecification().SortByRelevance());
            List<FilterCategoryVM> filterCategories = _productRepository.GetFilterCategoriesByProducts(products.ToList());
            products = _productRepository.SortProducts(products.ToList(), filters);
            List<ShowProductViewModel> showProducts = await _productRepository.FindProductsInTheCart(products.ToList(), User.Identity.Name);
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
            var products = await _productRepository.GetAllAsync(new ProductSpecification().SortByRelevance());
            products = _productRepository.SortProducts(products.ToList(), filters);
            List<ShowProductViewModel> showProducts = await _productRepository.FindProductsInTheCart(products.ToList(), User.Identity.Name);

            Thread.Sleep(1000);

            return Json(showProducts);
        }

        [HttpPost]
        [Route("Catalog/SearchAjax")]
        public async Task<IActionResult> SearchAjax(string q, List<string> filters)
        {
            var products = await _productRepository.SearchProductsAsync(q);
            products = _productRepository.SortProducts(products.ToList(), filters);
            List<ShowProductViewModel> showProducts = await _productRepository.FindProductsInTheCart(products.ToList(), User.Identity.Name);

            return Json(showProducts);
        }
    }
}
