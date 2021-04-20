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
        private readonly IShopCart _shopCart;

        public HomeController(IShopCart shopCart, IProductRepository IProductRepository, IUserRepository IUserRepository)
        {
            _userRepository = IUserRepository;
            _shopCart = shopCart;
            _productRepository = IProductRepository;
        }

        public async Task<ViewResult> News()
        {
            IEnumerable<CaruselItem> caruselItems = new CaruselItem[]
            {
                new CaruselItem
                {
                    ShortDesc = "Самый технологичный",
                    LongDesc = "С iphone 12 вы будете иметь все самые новые технологии современных смартфонов",
                    Img = "/img/iphone-12.jpg",
                },
                new CaruselItem
                {
                    ShortDesc = "Гибкий экран",
                    LongDesc = "С таким смартфоном на вас будут обращать внимание все",
                    Img = "/img/z-flip.jpg"
                },
                new CaruselItem
                {
                    ShortDesc = "Каждое фото - как с обложки",
                    LongDesc = "Делайте фотографии днём и ночью, с Huawei P10 вы будете богом или богиней Instagram",
                    Img = "/img/huawei-p10.png",
                }
            };
            var products = await _productRepository.GetProductsAsync();
            products = products.OrderByDescending(p => p.Available).ThenByDescending(p => p.IsFavourite).ThenByDescending(p => p.Id).ToList();
            List<ShopCartItem> shopCartItems = (await _shopCart.GetShopItemsAsync(new ShopCartSpecification().IncludeProduct().WhereUser(await _userRepository.GetUserAsync(User.Identity.Name)))).ToList();
            List<ShowProductViewModel> showProducts = _productRepository.DeleteIfInCart(products.ToList(), shopCartItems);
            List<News> news = new List<News>();
            for (int i = 0; i <= 11; i++)
            {
                news.Add(new News
                {
                    Name = "Название новости",
                    Desc = "Это более длинное поле с вспомогательным текстом как естественный ввод к дополнительному контенту. Этот контент может быть немного длиннее.",
                    Img = "/img/huawei-p10.png",
                    CreateData = DateTime.Now
                });
            }
            var homeProducts = new HomeViewModel
            {
                CaruselItems = caruselItems,
                FavProducts = showProducts.Take(8),
                NewsList = news.Take(8)
            };

            return View(homeProducts);
        }
        public ViewResult Index()
        {
            return View();
        }
    }
}
