using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Models;
using WebApplication1.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "courier")]
    public class CourierController : Controller
    {
        private readonly IAllOrders allOrders;
        private readonly ShopCart shopCart;

        public CourierController(IAllOrders allOrders, ShopCart shopCart)
        {
            this.allOrders = allOrders;
            this.shopCart = shopCart;
        }

        // GET: СourierController
        public ActionResult Index()
        {
            return View();
        }

        // GET: СourierController/Details/5
        public ActionResult AllOrders()
        {
            ViewBag.Title = "Все заказы";
            OrderViewModel orderR = new OrderViewModel
            {
                AllOrders = shopCart.GetAllOrders()
            };
            return View(orderR);
        }

        // GET: СourierController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: СourierController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: СourierController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: СourierController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: СourierController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: СourierController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
    }
}
