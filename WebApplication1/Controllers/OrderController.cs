using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class OrderController : Controller
    {
        private readonly IAllOrders allOrders;
        private readonly IShopCart newShopCart;
        private readonly ShopCart shopCart;

        public OrderController(IShopCart newShopCart, IAllOrders allOrders, ShopCart shopCart)
        {
            this.newShopCart = newShopCart;
            this.allOrders = allOrders;
            this.shopCart = shopCart;
        }

        public IActionResult Checkout()
        {
            ViewBag.Title = "Оформление заказа";
            var items = shopCart.GetShopItems();
            shopCart.ListShopItems = items;

            OrderViewModel orderR = new OrderViewModel
            {
                ShopCart = shopCart
            };
            shopCart.ListShopItems = shopCart.GetShopItems();

            return View(orderR);
        }

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            shopCart.ListShopItems = shopCart.GetShopItems();

            if (ModelState.IsValid)
            {
                allOrders.CreateOrder(order);
                return RedirectToAction("Complete");
            }

            var items = shopCart.GetShopItems();
            shopCart.ListShopItems = items;

            OrderViewModel orderR = new OrderViewModel
            {
                Order = order,
                ShopCart = shopCart,
            };
            shopCart.ListShopItems = shopCart.GetShopItems();


            return View(orderR);
        }

        public IActionResult Complete()
        {
            ViewBag.Title = "Завершение заказа";
            shopCart.ListShopItems = shopCart.GetShopItems();
            shopCart.EmptyTheCart(shopCart.ListShopItems);
            ViewBag.Message = "Заказ успешно обработан";
            return View();
        }
    }
}
