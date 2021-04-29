using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Models;
using WebApplication1.Data.Specifications;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly INewsRepository _newsRepository;

        public HomeController(IProductRepository IProductRepository, IUserRepository IUserRepository, INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
            _userRepository = IUserRepository;
            _productRepository = IProductRepository;
        }

        public async Task<ViewResult> News()
        {
            IEnumerable<CaruselItem> caruselItems = await _newsRepository.GetFavNewsAsync();
            var products = await _productRepository.GetProductsAsync(new ProductSpecification().SortByRelevance().Take(8));
            List<ShowProductViewModel> showProducts = await _productRepository.RemoveIfInCart(products.ToList(), User.Identity.Name);
            var news = await _newsRepository.GetNewsAsync(new NewsSpecification().SortById());
            var homeProducts = new HomeViewModel
            {
                CaruselItems = caruselItems,
                FavProducts = showProducts,
                NewsList = news.Take(8)
            };

            return View(homeProducts);
        }
        public ViewResult Index()
        {
            return View();
        }

        [Authorize(Roles = "admin, moderator")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken, Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> Create(CreateNewsViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _newsRepository.CreateNews(model);
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [Route("/News/{name}")]
        public async Task<IActionResult> Details(int id)
        {
            var news = await _newsRepository.GetNewsByIdAsync(id);
            return View(news);
        }

        [Authorize(Roles = "admin, moderator")]
        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken, Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> Edit()
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "admin, moderator")]
        public async Task<RedirectToActionResult> Delete(int id)
        {
            return RedirectToAction(nameof(Index));
        }
    }
}
