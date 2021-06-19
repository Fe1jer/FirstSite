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
    public class CatalogController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;

        public CatalogController(IProductRepository IProductRepository, IUserRepository IUserRepository)
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

        [Route("Catalog/Edit"), Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> Edit(int id)
        {

            User user = await _userRepository.GetUserAsync(User.Identity.Name);
            if (user.Role.Name != "admin" && user.Role.Name != "moderator")
            {
                return RedirectToAction("Logout", "Account");
            }

            Product obj = await _productRepository.GetByIdAsync(id);

            return View(obj);
        }

        [HttpPost, ValidateAntiForgeryToken, Route("Catalog/Edit")]
        public async Task<IActionResult> Edit(Product obj)
        {
            if (ModelState.IsValid)
            {
                await _productRepository.UpdateAsync(obj);

                return RedirectToAction("Index");
            }

            return View(obj);
        }

        [Route("Catalog/DeleteProduct"), Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            User user = await _userRepository.GetUserAsync(User.Identity.Name);
            if (user.Role.Name != "admin" && user.Role.Name != "moderator")
            {
                return RedirectToAction("Logout", "Account");
            }
            await _productRepository.DeleteAsync(id);

            /*            var products = await _productRepository.GetAllAsync();
                        foreach (Product product in products)
                        {
                            if (product.Name == "Test")
                            {
                                await _productRepository.DeleteAsync(product.Id);
                            }
                        }*/

            return RedirectToAction("Index");
        }

        [Route("Catalog/Create"), Authorize(Roles = "admin, moderator")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken, Route("Catalog/Create")]
        public async Task<IActionResult> Create(Product obj)
        {
            User user = await _userRepository.GetUserAsync(User.Identity.Name);
            if (user.Role.Name != "admin" && user.Role.Name != "moderator")
            {
                return RedirectToAction("Logout", "Account");
            }

            /*          Product product = new Product()
                        {
                            Name = "Test",
                            Available = false,
                            Category = "Test",
                            Company = "Test",
                            Country = "Test",
                            ShortDesc = "Test",
                            LongDesc = "Test",
                            IsFavourite = false,
                            Price = 0,
                            Img = "https://omoro.ru/wp-content/uploads/2018/08/syrikaty-2.jpg"
                        };
                        for (int i = 0; i <= 1000; i++)
                        {
                            product.Id = 0;
                            await _productRepository.AddProductAsync(product);
                        }*/

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

        [Route("Catalog/{name}")]
        public async Task<IActionResult> Product(int id, string name)
        {
            Product obj = await _productRepository.GetByIdAsync(id);

            if (obj != null)
            {
                ShowProductViewModel showProducts = await _productRepository.FindProductInTheCart(obj, User.Identity.Name);

                return View("Test", showProducts);
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
        [Route("Catalog/SearchAjax")]
        public async Task<IActionResult> SearchAjax(string q, List<string> filters)
        {
            var products = await _productRepository.SearchProductsAsync(q);
            products = _productRepository.SortProducts(products.ToList(), filters);
            List<ShowProductViewModel> showProducts = await _productRepository.FindProductsInTheCart(products.ToList(), User.Identity.Name);
            Thread.Sleep(100);

            return Json(showProducts);
        }
    }
}
