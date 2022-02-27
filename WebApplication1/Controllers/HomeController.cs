using AspNetCore.SEOHelper.Sitemap;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetShop.Data.Interfaces;
using InternetShop.Data.Models;
using InternetShop.Data.Specifications;
using InternetShop.ViewModels;

namespace InternetShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly INewsRepository _newsRepository;
        private readonly IShopCart _shopCart;
        private readonly ISiteRatingRepository _siteRatingRepository;
        private readonly IWebHostEnvironment _env;

        public HomeController(IProductRepository IProductRepository, IShopCart shopCart, INewsRepository newsRepository, ISiteRatingRepository ISiteRatingRepository, IWebHostEnvironment env)
        {
            _shopCart = shopCart;
            _newsRepository = newsRepository;
            _productRepository = IProductRepository;
            _siteRatingRepository = ISiteRatingRepository;
            _env = env;
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

        [Authorize(Roles = "moderator")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken, Authorize(Roles = "moderator")]
        public async Task<IActionResult> Create(CreateNewsViewModel model)
        {
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

        [Authorize(Roles = "moderator")]
        public async Task<IActionResult> Edit(int id)
        {
            News news = await _newsRepository.GetByIdAsync(id);
            ChangeNewsViewModel model = CreateChangeNewsModel(news);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken, Authorize(Roles = "moderator")]
        public async Task<IActionResult> Edit(ChangeNewsViewModel news)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _newsRepository.UpdateAsync(news);

                    return RedirectToAction(nameof(News));
                }
            }
            catch (Exception ex)
            {
                return View(news);
            }

            return View(news);
        }

        [Authorize(Roles = "moderator")]
        public async Task<RedirectToActionResult> Delete(int id)
        {
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
        public async Task SitemapXml()
        {
            var list = new List<SitemapNode>
            {
                new SitemapNode { LastModified = DateTime.UtcNow, Priority = 1, Url = Url.ActionLink("Index", "Home"), Frequency = SitemapFrequency.Weekly   },
                new SitemapNode { LastModified = DateTime.UtcNow, Priority = 0.9, Url = Url.ActionLink("News", "Home"), Frequency = SitemapFrequency.Weekly   },
                new SitemapNode { LastModified = DateTime.UtcNow, Priority = 1, Url = Url.ActionLink("Index", "Catalog"), Frequency = SitemapFrequency.Weekly   },
                new SitemapNode { LastModified = DateTime.UtcNow, Priority = 0, Url = Url.ActionLink("Search", "Catalog"), Frequency = SitemapFrequency.Weekly   },
                new SitemapNode { LastModified = DateTime.UtcNow, Priority = 0.7, Url = Url.ActionLink("Login", "Account"), Frequency = SitemapFrequency.Weekly   },
                new SitemapNode { LastModified = DateTime.UtcNow, Priority = 0.7, Url = Url.ActionLink("Register", "Account"), Frequency = SitemapFrequency.Weekly   },
            };
            foreach (var news in await _newsRepository.GetAllAsync())
            {
                if (news.ProductHref == null)
                {
                    SitemapNode sitemapNode = new SitemapNode
                    {
                        LastModified = DateTime.UtcNow,
                        Priority = 0.6,
                        Url = Url.ActionLink("Index") + "News/" + news.Name + "?id=" + news.Id,
                        Frequency = SitemapFrequency.Weekly
                    };
                    list.Add(sitemapNode);
                }
            }
            foreach (var product in await _productRepository.GetAllAsync())
            {
                SitemapNode sitemapNode = new SitemapNode
                {
                    LastModified = DateTime.UtcNow,
                    Priority = 0.8,
                    Url = Url.ActionLink("Index") + "Catalog/" + product.ProductType.Name.Replace(" ", "-") + "?id=" + product.Id,
                    Frequency = SitemapFrequency.Weekly
                };
                list.Add(sitemapNode);
            }

            new SitemapDocument().CreateSitemapXML(list, _env.ContentRootPath);
        }
    }
}
