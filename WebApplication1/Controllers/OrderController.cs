using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class OrderController : Controller
    {
        private readonly IAllOrders allOrders;
        private readonly IShopCart shopCart;
        private readonly IAllUsers _allUser;

        public OrderController(IShopCart shopCart, IAllOrders allOrders, IAllUsers allUser)
        {
            _allUser = allUser;
            this.shopCart = shopCart;
            this.allOrders = allOrders;
        }

        public IActionResult Checkout()
        {
            ViewBag.Title = "Оформление заказа";

            OrderViewModel orderR = new OrderViewModel
            {
                ShopCartItems = shopCart.GetShopItems(_allUser.GetUserEmail(User.Identity.Name))
            };

            return View(orderR);
        }

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if (ModelState.IsValid)
            {
                allOrders.CreateOrder(_allUser.GetUserEmail(User.Identity.Name), order);
                return RedirectToAction("Complete");
            }
            OrderViewModel orderR = new OrderViewModel
            {
                Order = order,
                ShopCartItems = shopCart.GetShopItems(_allUser.GetUserEmail(User.Identity.Name))
            };

            return View(orderR);
        }

        public IActionResult Complete()
        {
            ViewBag.Title = "Завершение заказа";
            shopCart.EmptyTheCart(_allUser.GetUserEmail(User.Identity.Name));
            ViewBag.Message = "Заказ успешно обработан";
            return View();
        }
    }
}
