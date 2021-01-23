using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAllCars _carRep;

        public HomeController(IAllCars carRep)
        {
            _carRep = carRep;
        }

        public ViewResult Catalog()
        {
            var homeCars = new HomeViewModel
            {
                FavCars = _carRep.GetFavCars
            };
            return View(homeCars);
        }
        public ViewResult Index()
        {
            return View();
        }
    }
}
