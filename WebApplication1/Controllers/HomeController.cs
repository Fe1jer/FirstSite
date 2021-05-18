using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
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
        private readonly IShopCart _shopCart;
        private readonly ISiteRatingRepository _siteRatingRepository;

        public HomeController(IUserRepository IUserRepository, IProductRepository IProductRepository, IShopCart shopCart, INewsRepository newsRepository, ISiteRatingRepository ISiteRatingRepository)
        {
            _userRepository = IUserRepository;
            _shopCart = shopCart;
            _newsRepository = newsRepository;
            _productRepository = IProductRepository;
            _siteRatingRepository = ISiteRatingRepository;
        }

        public async Task<ViewResult> News()
        {
            IEnumerable<CaruselItem> caruselItems = await _newsRepository.GetFavNewsAsync();
            var userShopCartItems = await _shopCart.GetAllAsync(new ShopCartSpecification().WhereUserEmail(User.Identity.Name));
            var products = await _productRepository.GetAllAsync(new ProductSpecification().SortByRelevance().WhereNotOnTheList(userShopCartItems.Select(p => p.Product).ToList()).Take(8));
            List<ShowProductViewModel> showProducts = await _productRepository.FindProductsInTheCart(products.ToList(), User.Identity.Name);
            var news = await _newsRepository.GetNewsAsync(new NewsSpecification().SortById().Take(8));
            var homeProducts = new NewsViewModel
            {
                CaruselItems = caruselItems,
                FavProducts = showProducts,
                NewsList = news
            };

            return View(homeProducts);
        }
        public async Task<ViewResult> Index()
        {
            double rating = await _siteRatingRepository.OverallSiteRating();
            HomeViewModel model = new HomeViewModel()
            {
                Rating = rating
            };
            return View(model);
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
            User user = await _userRepository.GetUserAsync(User.Identity.Name);
            if (user.Role.Name != "admin" && user.Role.Name != "moderator")
            {
                return RedirectToAction("Logout", "Account");
            }
            if (ModelState.IsValid)
            {
                await _newsRepository.CreateAsync(model);
                return RedirectToAction(nameof(News));
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
            ChangeNewsViewModel model = CreateChangeNewsModel(news);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken, Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> Edit(ChangeNewsViewModel news)
        {
            User user = await _userRepository.GetUserAsync(User.Identity.Name);
            if (user.Role.Name != "admin" && user.Role.Name != "moderator")
            {
                return RedirectToAction("Logout", "Account");
            }
            try
            {
                if (ModelState.IsValid)
                {
                    await _newsRepository.UpdateAsync(news);

                    return RedirectToAction(nameof(News));
                }
            }
            catch
            {
                return View(news);
            }

            return View(news);
        }

        [Authorize(Roles = "admin, moderator")]
        public async Task<RedirectToActionResult> Delete(int id)
        {
            User user = await _userRepository.GetUserAsync(User.Identity.Name);
            if (user.Role.Name != "admin" && user.Role.Name != "moderator")
            {
                return RedirectToAction("Logout", "Account");
            }
            await _newsRepository.DeleteAsync(id);
            return RedirectToAction(nameof(News));
        }

        private ChangeNewsViewModel CreateChangeNewsModel(News news)
        {
            ChangeNewsViewModel model = new ChangeNewsViewModel
            {
                Id = news.Id,
                Desc = news.Desc,
                Img = news.Img,
                Name = news.Name,
            };

            if (news.Text != null)
            {
                model.Text = news.Text;
            }
            else
            {
                model.IsProductHref = true;
                model.ProductHref = news.ProductHref;
            }
            if (news.FavImg != null)
            {
                model.IsCaruselNews = true;
                model.FavImg = news.FavImg;
            }

            return model;
        }
        [Route("/sitemap.xml")]
        public async void SitemapXml()
        {
            string host = Request.Scheme + "://" + Request.Host;

            Response.ContentType = "application/xml";

            using var xml = XmlWriter.Create(Response.Body, new XmlWriterSettings { Indent = true });
            await xml.WriteStartDocumentAsync();
            xml.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");

            xml.WriteStartElement("url");
            xml.WriteElementString("loc", host);
            xml.WriteEndElement();

            xml.WriteEndElement();
        }
    }
}
