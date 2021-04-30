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
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly IShopCart _shopCart;

        public ShopCartController(IShopCart shopCart, IProductRepository IProductRepository, IUserRepository IUserRepository)
        {
            _shopCart = shopCart;
            _userRepository = IUserRepository;
            _productRepository = IProductRepository;
        }

        public async Task<ViewResult> Index()
        {
            var shopCartItems = await _shopCart.GetAllAsync(new ShopCartSpecification().WhereUserEmail(User.Identity.Name));

            return View(shopCartItems);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int IdProduct)
        {
            if (IdProduct == 0)
            {
                return View("PageNotFound");
            }
            Product item = await _productRepository.GetByIdAsync(IdProduct);
            if (item != null)
            {
                await _shopCart.AddAsync(User.Identity.Name, item);
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
            await _shopCart.DeleteAsync(User.Identity.Name, IdProduct);
            var shopCartItems = await _shopCart.GetAllAsync(new ShopCartSpecification().WhereUserEmail(User.Identity.Name));

            return Json(shopCartItems.Count);
        }
    }
}
