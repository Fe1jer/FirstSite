using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Models;
using WebApplication1.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class ShopCartController : Controller
    {
        private readonly IAllProduct _prooductRep;
        private readonly IAllUsers _allUser;
        private readonly IShopCart _shopCart;

        public ShopCartController(IShopCart shopCart, IAllProduct prooductRep, IAllUsers allUser)
        {
            _shopCart = shopCart;
            _allUser = allUser;
            _prooductRep = prooductRep;
        }

        public ViewResult Index()
        {
            var obj = _shopCart.GetShopItems(_allUser.GetUserEmail(User.Identity.Name));
            ViewBag.Title = "Корзина";

            return View(obj);
        }
 
        public RedirectToActionResult RemoveToCart(int IdProduct)
        {
            _shopCart.RemoveToCart(_allUser.GetUserEmail(User.Identity.Name), IdProduct);

            return RedirectToAction("Index");
        }
    }
}
