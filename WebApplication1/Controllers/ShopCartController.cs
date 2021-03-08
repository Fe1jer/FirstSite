using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Models;
using WebApplication1.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace WebApplication1.Controllers
{
    public class ShopCartController : Controller
    {
        private readonly IAllProduct _prooductRep;
        private readonly ShopCart _shopCart;
        private readonly IAllUser _allUser;
        private readonly IShopCart _newShopCart;

        public ShopCartController(IShopCart newShopCart, IAllProduct prooductRep, ShopCart shopCart, IAllUser allUser)
        {
            _newShopCart = newShopCart;
            _allUser = allUser;
            _prooductRep = prooductRep;
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
        public RedirectToActionResult RemoveToCart(string IdProduct)
        {
            _shopCart.RemoveToCart(IdProduct);

            return RedirectToAction("Index");
        }
    }
}
