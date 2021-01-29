using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Models;
using WebApplication1.ViewModels;
using System.Drawing;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAllProduct _productRep;

        public HomeController(IAllProduct productRep)
        {
            _productRep = productRep;
        }

        public interface IEnumerator
        {
            bool MoveNext(); // перемещение на одну позицию вперед в контейнере элементов
            object Current { get; }  // текущий элемент в контейнере
            void Reset(); // перемещение в начало контейнера
        }

        public ViewResult Catalog()
        {
            var homeProducts = new HomeViewModel
            {
                AutoOnTheCover = new HomeProduct[]
                {
                    new HomeProduct
                     {
                         ShortDesc = "Бесшумный и экономный",
                         LongDesc = "Небольшой семейный автомобиль для городской жизни",
                         Img = "/img/volkswagen.jpg",
                     },
                      new HomeProduct
                     {
                        ShortDesc = "Дерзкий и стильный",
                        LongDesc = "Удобный автомобиль для городской жизни",
                         Img = "/img/bmwM3.jpg",
                     },
                       new HomeProduct
                     {
                         ShortDesc = "Современный и большой",
                        LongDesc = "Премиальный автомобиль для городской жизни",
                         Img = "/img/teslamodelx.jpg",
                       }
                },
                FavProducts = _productRep.GetFavProducts
            };
            ViewBag.Title = "Главная страница";

            return View(homeProducts);
        }
        public ViewResult Index()
        {
            ViewBag.Title = "Обложка";

            return View();
        }
    }
}
