using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Models;
using WebApplication1.Data.Specifications;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "courier, moderator")]
    public class CourierController : Controller
    {
        private readonly IOrdersRepository allOrders;
        private readonly IUserRepository allUser;

        public CourierController(IOrdersRepository allOrders, IUserRepository allUser)
        {
            this.allOrders = allOrders;
            this.allUser = allUser;
        }

        [Authorize(Roles = "moderator")]
        // GET: СourierController
        public async Task<ActionResult> Index()
        {
            ViewBag.Title = "Все заказы";
            return View(await allOrders.GetOrdersAsync(
                new OrderSpecification().
                IncludeDetails().IncludeCourier().SortByCourier().
                WithoutTracking()
                ));
        }

        [Authorize(Roles = "courier")]
        public async Task<ActionResult> CourierOrders()
        {
            ViewBag.Title = "Заказы";

            return View(await allOrders.GetOrdersAsync(new OrderSpecification()
                .WhereCourierEmail(User.Identity.Name)
                .IncludeCourier()
                .IncludeDetails()
                .WithoutTracking()));
        }

        [Authorize(Roles = "courier")]
        public async Task<ActionResult> Renouncement(int idOrder)
        {
            ViewBag.Title = "Заказы";
            await allOrders.UpdateCourierOrdersAsync(idOrder, null);
            return RedirectToAction("CourierOrders");
        }

        [Authorize(Roles = "moderator")]
        public async Task<ActionResult> Edit(int idOrder)
        {
            ChangeCourierViewModels model = new ChangeCourierViewModels
            {
                Order = await allOrders.GetOrderByIdAsync(idOrder),
                AllCouriers = await allUser.GetUsersAsync(new UserSpecification().IncludeRole().WhereRole("courier"))
            };
            ViewBag.Title = "Выбор курьера";

            return View(model);
        }

        [Authorize(Roles = "moderator")]
        [HttpPost]
        public async Task<ActionResult> Edit(int idOrder, int idCourier)
        {
            ViewBag.Title = "Выбор курьера";
            User courier = await allUser.GetUserAsync(idCourier);
            await allOrders.UpdateCourierOrdersAsync(idOrder, courier);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> OrderDetail(int idOrder)
        {
            ViewBag.Title = "Информация о заказе";

            return View(await allOrders.GetOrderByIdAsync(idOrder));
        }

        public async Task<ActionResult> Delete(int idOrder)
        {
            ViewBag.Title = "Информация о заказе";
            await allOrders.DeleteOrderAsync(await allOrders.GetOrderByIdAsync(idOrder));
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
