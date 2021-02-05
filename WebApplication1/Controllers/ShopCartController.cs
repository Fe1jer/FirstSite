﻿using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Models;
using WebApplication1.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace WebApplication1.Controllers
{
    public class ShopCartController : Controller
    {
        private readonly IAllProduct _carRep;
        private readonly ShopCart _shopCart;

        public ShopCartController(IAllProduct carRep, ShopCart shopCart)
        {
            _carRep = carRep;
            _shopCart = shopCart;
        }

        [Authorize(Roles = "admin, user")]
        public ViewResult Index()
        {
            var items = _shopCart.GetShopItems();
            _shopCart.ListShopItems = items;
            var obj = new ShopCartViewModel
            {
                ShopCart = _shopCart
            };
            ViewBag.Title = "Корзина";

            return View(obj);
        }

        [Authorize(Roles = "admin, user")]  
        public RedirectToActionResult RemoveToCart(string IdCar)
        {
            _shopCart.RemoveToCart(IdCar);

            return RedirectToAction("Index");
        }
    }
}
