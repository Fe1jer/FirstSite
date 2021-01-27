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
    public class OrderController : Controller
    {
        private readonly IAllOrders allOrders;
        private readonly ShopCart shopCart;

        public OrderController(IAllOrders allOrders, ShopCart shopCart)
        {
            this.allOrders = allOrders;
            this.shopCart = shopCart;
        }

        public IActionResult Checkout()
        {
            var items = shopCart.GetShopItems();
            shopCart.ListShopItems = items;

            OrderViewModel orderR = new OrderViewModel
            {
                ShopCart = shopCart,
            };
            shopCart.ListShopItems = shopCart.GetShopItems();

            return View(orderR);
        }

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            shopCart.ListShopItems = shopCart.GetShopItems();

            if (shopCart.ListShopItems.Count == 0)
            {
                ModelState.AddModelError("", "Нет товаров в корзине");
            }

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
            ViewBag.Message = "Заказ успешно обработан";
            return View();
        }
    }
}
