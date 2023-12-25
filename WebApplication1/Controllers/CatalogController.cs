using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetShop.Data.Interfaces;
using InternetShop.Data.Models;
using InternetShop.Data.Specifications;
using InternetShop.ViewModels;
using ReflectionIT.Mvc.Paging;
using Microsoft.AspNetCore.Routing;

namespace InternetShop.Controlles
{
    public class CatalogController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly int _numProductsPerPage;

        public CatalogController(IProductRepository IProductRepository)
        {
            _productRepository = IProductRepository;
            _numProductsPerPage = 12;
        }

        [HttpGet, Route("Catalog/Search")]
        public async Task<IActionResult> Search(string q, List<string> filters, int? page)
        {
            var products = await _productRepository.SearchProductsAsync(q);
            List<FilterCategoryVM> filterCategories = _productRepository.GetFilterCategoriesByProducts(products.ToList());
            products = _productRepository.SortProducts(products.ToList(), filters);
            List<ShowProductViewModel> showProducts = await _productRepository.FindProductsInTheCart(products.ToList(), User.Identity.Name);

            var productsPagingList = PagingList.Create(showProducts, _numProductsPerPage, page ?? 1);
            productsPagingList.Action = "Search";
            productsPagingList.RouteValue = new RouteValueDictionary {
                { "q", q},
            };
            int i = 0;
            foreach (string filter in filters)
            {
                productsPagingList.RouteValue.Add($"filters[{i}]", filter);
                i++;
            }
            var productObj = new ProductsListViewModel
            {
                AllProducts = productsPagingList,
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

        [Route("Catalog/Edit"), Authorize(Roles = "moderator")]
        public async Task<IActionResult> Edit(int id)
        {
            Product obj = await _productRepository.GetByIdAsync(id);
            CreateChangeProductViewModel model = new CreateChangeProductViewModel()
            {
                Product = obj
            };
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, Route("Catalog/Edit")]
        public async Task<IActionResult> Edit(CreateChangeProductViewModel obj)
        {
            if (ModelState.IsValid)
            {
                await _productRepository.UpdateAsync(obj);

                return RedirectToAction("Index");
            }

            return View(obj);
        }

        [Route("Catalog/DeleteProduct"), Authorize(Roles = "moderator")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productRepository.DeleteAsync(id);

            return RedirectToAction("Index");
        }

        [Route("Catalog/Create"), Authorize(Roles = "moderator")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken, Route("Catalog/Create")]
        public async Task<IActionResult> Create(CreateChangeProductViewModel obj)
        {
            if (ModelState.IsValid)
            {
                await _productRepository.AddProductAsync(obj);

                return RedirectToAction("Index");
            }

            return View(obj);
        }

        [Route("Catalog/{name}")]
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
        public async Task<IActionResult> Index(List<string> filters, int? page)
        {
            var products = await _productRepository.GetAllAsync(new ProductSpecification().SortByRelevance());
            List<FilterCategoryVM> filterCategories = _productRepository.GetFilterCategoriesByProducts(products.ToList());
            products = _productRepository.SortProducts(products.ToList(), filters);
            List<ShowProductViewModel> showProducts = await _productRepository.FindProductsInTheCart(products.ToList(), User.Identity.Name);

            var productsPagingList = PagingList.Create(showProducts, _numProductsPerPage, page ?? 1);
            if (filters != null)
            {
                int i = 0;

                productsPagingList.RouteValue = new RouteValueDictionary();
                foreach (string filter in filters)
                {
                    productsPagingList.RouteValue.Add($"filters[{i}]", filter);
                    i++;
                }
            }
            var productObj = new ProductsListViewModel
            {
                AllProducts = productsPagingList,
                FilterSort = filterCategories,
                Filter = filters
            };

            return View(productObj);
        }

        [HttpPost]
        [Route("Catalog/GetPartialSearchProduct")]
        public async Task<IActionResult> GetPartialSearchProduct(string q, List<string> filters, int? page)
        {
            var products = await _productRepository.SearchProductsAsync(q);
            products = _productRepository.SortProducts(products.ToList(), filters);
            List<ShowProductViewModel> showProducts = await _productRepository.FindProductsInTheCart(products.ToList(), User.Identity.Name);
            var productsPagingList = PagingList.Create(showProducts, _numProductsPerPage, page ?? 1);
            if (q != null)
            {
                productsPagingList.Action = "Search";
                productsPagingList.RouteValue = new RouteValueDictionary { { "q", q } };
            }
            else if (filters != null)
            {
                productsPagingList.RouteValue = new RouteValueDictionary();
            }
            int i = 0;
            foreach (string filter in filters)
            {
                productsPagingList.RouteValue.Add($"filters[{i}]", filter);
                i++;
            }
            return PartialView("ProductsListPartial", productsPagingList);
        }

        [HttpPost]
        [Route("Catalog/GetSearchProduct")]
        public async Task<IActionResult> GetSearchProduct(string q)
        {
            var products = await _productRepository.SearchProductsAsync(q);
            return Json(products);
        }
    }
}
