using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Models;
using WebApplication1.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "courier, moderator")]
    public class CourierController : Controller
    {
        private readonly IAllOrders allOrders;
        private readonly IAllUsers allUser;

        public CourierController(IAllOrders allOrders, IAllUsers allUser)
        {
            this.allOrders = allOrders;
            this.allUser = allUser;
        }

        [Authorize(Roles = "moderator")]
        // GET: СourierController
        public ActionResult Index()
        {
            ViewBag.Title = "Все заказы";

            return View(allOrders.GetAllOrders());
        }

        [Authorize(Roles = "courier")]
        public ActionResult CourierOrders()
        {
            ViewBag.Title = "Заказы";

            return View(allOrders.GetCourierOrders(User.Identity.Name));
        }

        [Authorize(Roles = "courier")]
        public ActionResult Renouncement(int idOrder)
        {
            ViewBag.Title = "Заказы";
            allOrders.SetCourierOrders(idOrder, 0);
            return RedirectToAction("CourierOrders");
        }

        [Authorize(Roles = "moderator")]
        public ActionResult Edit(int idOrder)
        {
            ChangeCourierViewModels model = new ChangeCourierViewModels
            {
                Order = allOrders.GetOrder(idOrder),
                AllCouriers = allUser.Couriers
            };
            ViewBag.Title = "Выбор курьера";
             
            return View(model);
        }

        [Authorize(Roles = "moderator")]
        [HttpPost]
        public ActionResult Edit(int idOrder, int idCourier)
        {
            ViewBag.Title = "Выбор курьера";
            allOrders.SetCourierOrders(idOrder, idCourier);

            return RedirectToAction("Index");
        }

        public ActionResult OrderDetail(int idOrder)
        {
            ViewBag.Title = "Информация о заказе";

            return View(allOrders.GetOrder(idOrder));
        }

        public ActionResult Delete(int idOrder)
        {
            ViewBag.Title = "Информация о заказе";
            allOrders.DeleteOrder(idOrder);
            if (User.IsInRole("moderator"))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("CourierOrders");
            }
        }
    }
}
