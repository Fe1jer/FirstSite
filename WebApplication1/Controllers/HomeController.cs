using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IProductRepository _productRepository;
        private readonly INewsRepository _newsRepository;
        private readonly IShopCart _shopCart;

        public HomeController(IProductRepository IProductRepository, IShopCart shopCart, INewsRepository newsRepository)
        {
            _shopCart = shopCart;
            _newsRepository = newsRepository;
            _productRepository = IProductRepository;
        }

        public async Task<ViewResult> News()
        {
            IEnumerable<CaruselItem> caruselItems = await _newsRepository.GetFavNewsAsync();
            var userShopCartItems = await _shopCart.GetAllAsync(new ShopCartSpecification().WhereUserEmail(User.Identity.Name));
            var products = await _productRepository.GetAllAsync(new ProductSpecification().SortByRelevance().WhereNotOnTheList(userShopCartItems.Select(p=>p.Product).ToList()).Take(8));
            List<ShowProductViewModel> showProducts = await _productRepository.FindProductsInTheCart(products.ToList(), User.Identity.Name);
            var news = await _newsRepository.GetNewsAsync(new NewsSpecification().SortById().Take(8));
            var homeProducts = new HomeViewModel
            {
                CaruselItems = caruselItems,
                FavProducts = showProducts,
                NewsList = news
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
            var news = await _newsRepository.GetByIdAsync(id);
            return View(news);
        }

        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> Edit(int id)
        {
            News news = await _newsRepository.GetByIdAsync(id);
            return View(news);
        }

        [HttpPost]
        [ValidateAntiForgeryToken, Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> Edit(News news)
        {
            try
            {
                await _newsRepository.UpdateAsync(news);
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
            await _newsRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
