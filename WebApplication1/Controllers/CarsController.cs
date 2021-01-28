using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controlles
{
    public class CarsController : Controller
    {
        private readonly IAllCars _allCars;
        private readonly ICarsCategory _allCategory;
        private readonly ShopCart _shopCart;


        public CarsController(IAllCars iAllCars, ICarsCategory iCarsCat, ShopCart shopCart)
        {
            _allCars = iAllCars;
            _allCategory = iCarsCat;
            _shopCart = shopCart;
        }

        [Route("Cars/List")]
        [Route("Cars/List/{category}")]
        public ViewResult List(string category)
        {
            string _category = category;
            IEnumerable<Car> cars = null;
            string currCategory = "";
            if (string.IsNullOrEmpty(category))
            {
                cars = _allCars.Cars.OrderBy(i => i.Id);
            }
            else
            {
                if (string.Equals("Electro", category, StringComparison.OrdinalIgnoreCase))
                {
                    cars = _allCars.Cars.Where(i => i.Category.CategoryName.Equals("Электромобиль")).OrderBy(i => i.Id);
                    currCategory = "Электромобили";
                }
                else if (string.Equals("Fuel", category, StringComparison.OrdinalIgnoreCase))
                {
                    cars = _allCars.Cars.Where(i => i.Category.CategoryName.Equals("Автомобиль с ДВС")).OrderBy(i => i.Id);
                    currCategory = "Автомобили с ДВС";
                }
            }

            var carObj = new CarsListViewModel
            {
                AllCars = cars,
                CurrCategory = currCategory,
                ShopCart = _shopCart
            };

            ViewBag.Title = "Страница с автомобилями";

            return View(carObj);
        }

        public RedirectToActionResult AddToCart(int id)
        {
            var item = _allCars.Cars.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                _shopCart.AddToCart(item);
            }

            return RedirectToAction("List");
        }

        public RedirectToActionResult RemoveToCart(string id)
        {
            _shopCart.RemoveToCart(id);

            return RedirectToAction("List");
        }
    }
}
