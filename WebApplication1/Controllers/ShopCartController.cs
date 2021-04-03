using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Specifications;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class ShopCartController : Controller
    {
        private readonly IProductRepository _prooductRep;
        private readonly IUserRepository _allUser;
        private readonly IShopCart _shopCart;

        public ShopCartController(IShopCart shopCart, IProductRepository prooductRep, IUserRepository allUser)
        {
            _shopCart = shopCart;
            _allUser = allUser;
            _prooductRep = prooductRep;
        }

        public async Task<ViewResult> Index()
        {
            var obj = await _shopCart.GetShopItemsAsync(new ShopCartSpecification().IncludeProduct().WhereUser(await _allUser.GetUserAsync(User.Identity.Name)));

            return View(obj);
        }

        public async Task<RedirectToActionResult> RemoveToCart(int IdProduct)
        {
            await _shopCart.RemoveToCart(await _allUser.GetUserAsync(User.Identity.Name), IdProduct);

            return RedirectToAction("Index");
        }
    }
}
