using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using InternetShop.Data.Interfaces;
using InternetShop.Data.Models;
using InternetShop.Data.Specifications;
using InternetShop.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace InternetShop.Controllers
{
    [Authorize(Roles = "courier, moderator")]
    public class CourierController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IOrdersRepository _ordersRepository;

        public CourierController(IOrdersRepository IOrdersRepository, UserManager<User> userManager)
        {
            _ordersRepository = IOrdersRepository;
            _userManager = userManager;
        }

        [Authorize(Roles = "moderator")]
        public async Task<ActionResult> Index()
        {
            return View(await _ordersRepository.GetAllAsync(
                new OrderSpecification()
                .IncludeCourier()
                .SortByCourier()
                .WhereActual()
                .WithoutTracking()));
        }

        [Authorize(Roles = "courier")]
        public async Task<ActionResult> CourierOrders()
        {
            return View(await _ordersRepository.GetAllAsync(new OrderSpecification()
                .WhereCourierEmail(User.Identity.Name)
                .IncludeCourier()
                .WhereActual()
                .WithoutTracking()));
        }

        [Authorize(Roles = "courier")]
        public async Task<ActionResult> Renouncement(int idOrder)
        {
            await _ordersRepository.UpdateCourierOrdersAsync(idOrder, null);
            return RedirectToAction("CourierOrders");
        }

        [Authorize(Roles = "moderator")]
        public async Task<ActionResult> Edit(int idOrder)
        {
            ChangeCourierViewModels model = new ChangeCourierViewModels
            {
                Order = await _ordersRepository.GetByIdAsync(idOrder),
                AllCouriers = await _userManager.GetUsersInRoleAsync("courier")
            };

            return View(model);
        }

        [Authorize(Roles = "moderator")]
        [ValidateAntiForgeryToken, HttpPost]
        public async Task<ActionResult> Edit(int idOrder, string idCourier)
        {
            User courier = await _userManager.FindByIdAsync(idCourier);
            await _ordersRepository.UpdateCourierOrdersAsync(idOrder, courier);
            return RedirectToAction("Index");
        }

        public ActionResult OrderDetail(int idOrder)
        {
            Order order = _ordersRepository.GetAllAsync(new OrderSpecification()
                .WithoutTracking()).Result.Where(p => p.Id == idOrder).FirstOrDefault();
            return View(order);
        }

        public async Task<ActionResult> Delete(int idOrder)
        {
            await _ordersRepository.CompletedOrderAsync(await _ordersRepository.GetByIdAsync(idOrder));
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
