using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Models;
using WebApplication1.Data.Specifications;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrdersRepository allOrders;
        private readonly IShopCart shopCart;
        private readonly IUserRepository _allUser;

        public OrderController(IShopCart shopCart, IOrdersRepository allOrders, IUserRepository allUser)
        {
            _allUser = allUser;
            this.shopCart = shopCart;
            this.allOrders = allOrders;
        }

        public async Task<IActionResult> Checkout()
        {
            ViewBag.Title = "Оформление заказа";

            OrderViewModel orderR = new OrderViewModel
            {
                ShopCartItems = (List<ShopCartItem>)await shopCart.GetShopItemsAsync(new ShopCartSpecification().IncludeProduct().WhereUser(await _allUser.GetUserAsync(User.Identity.Name)))
        };

            return View(orderR);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(Order order)
        {
            if (ModelState.IsValid)
            {
               await allOrders.AddOrder(await _allUser.GetUserAsync(User.Identity.Name), order);
                return RedirectToAction("Complete");
            }
            OrderViewModel orderR = new OrderViewModel
            {
                Order = order,
                ShopCartItems = (List<ShopCartItem>)await shopCart.GetShopItemsAsync(new ShopCartSpecification().IncludeProduct().WhereUser(await _allUser.GetUserAsync(User.Identity.Name)))
            };

            return View(orderR);
        }

        public async Task<IActionResult> Complete()
        {
            ViewBag.Title = "Завершение заказа";
            shopCart.EmptyTheCart(await _allUser.GetUserAsync(User.Identity.Name));
            ViewBag.Message = "Заказ успешно обработан";
            return View();
        }
    }
}
