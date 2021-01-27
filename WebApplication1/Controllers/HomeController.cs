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
        private readonly IAllCars _carRep;

        public HomeController(IAllCars carRep)
        {
            _carRep = carRep;
        }

        public interface IEnumerator
        {
            bool MoveNext(); // перемещение на одну позицию вперед в контейнере элементов
            object Current { get; }  // текущий элемент в контейнере
            void Reset(); // перемещение в начало контейнера
        }

        public ViewResult Catalog()
        {
            var homeCars = new HomeViewModel
            {
                AutoOnTheCover = new HomeCar[]
                {
                    new HomeCar
                     {
                         ShortDesc = "Бесшумный и экономный",
                         LongDesc = "Небольшой семейный автомобиль для городской жизни",
                         Img = "/img/volkswagen.jpg",
                     },
                      new HomeCar
                     {
                        ShortDesc = "Дерзкий и стильный",
                        LongDesc = "Удобный автомобиль для городской жизни",
                         Img = "/img/bmwM3.jpg",
                     },
                       new HomeCar
                     {
                         ShortDesc = "Современный и большой",
                        LongDesc = "Премиальный автомобиль для городской жизни",
                         Img = "/img/teslamodelx.jpg",
                       }
                },
                FavCars = _carRep.GetFavCars
            };
            ViewBag.Title = "Главная страница";

            return View(homeCars);
        }
        public ViewResult Index()
        {
            ViewBag.Title = "Обложка";

            return View();
        }
    }
}
