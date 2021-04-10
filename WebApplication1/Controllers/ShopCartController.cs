using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Models;
using WebApplication1.Data.Specifications;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class ShopCartController : Controller
    {
        private readonly IProductRepository _allProducts;
        private readonly IUserRepository _allUser;
        private readonly IShopCart _shopCart;

        public ShopCartController(IShopCart shopCart, IProductRepository prooductRep, IUserRepository allUser)
        {
            _shopCart = shopCart;
            _allUser = allUser;
            _allProducts = prooductRep;
        }

        public async Task<ViewResult> Index()
        {
            var shopCartItems = await _shopCart.GetShopItemsAsync(new ShopCartSpecification().IncludeProduct().WhereUser(await _allUser.GetUserAsync(User.Identity.Name)));

            return View(shopCartItems);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int IdProduct)
        {
            if (IdProduct == 0)
            {
                return View("PageNotFound");
            }
            Product item = await _allProducts.GetProductByIdAsync(IdProduct);
            if (item != null)
            {
                await _shopCart.AddToCart(await _allUser.GetUserAsync(User.Identity.Name), item);
            }

            return new EmptyResult();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveToCart(int IdProduct)
        {
            if (IdProduct == 0)
            {
                return View("PageNotFound");
            }
            await _shopCart.RemoveToCart(await _allUser.GetUserAsync(User.Identity.Name), IdProduct);

            var shopCartItems = await _shopCart.GetShopItemsAsync(new ShopCartSpecification().IncludeProduct().WhereUser(await _allUser.GetUserAsync(User.Identity.Name)));

            return Json(shopCartItems.Count);
        }
    }
}
