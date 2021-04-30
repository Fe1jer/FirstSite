using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Models;
using WebApplication1.Data.Specifications;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly IShopCart _shopCart;
        private readonly IUserRepository _productRepository;

        public OrderController(IShopCart shopCart, IOrdersRepository IOrdersRepository, IUserRepository IProductRepository)
        {
            _productRepository = IProductRepository;
            _shopCart = shopCart;
            _ordersRepository = IOrdersRepository;
        }

        public async Task<IActionResult> Checkout()
        {
            OrderViewModel orderR = new OrderViewModel
            {
                ShopCartItems = (List<ShopCartItem>)await _shopCart.GetAllAsync(new ShopCartSpecification().WhereUserEmail(User.Identity.Name))
            };

            return View(orderR);
        }

        [ValidateAntiForgeryToken, HttpPost]
        public async Task<IActionResult> Checkout(Order order, int countShopCartItems)
        {
            if (ModelState.IsValid)
            {
                if (countShopCartItems != 0)
                {
                    await _ordersRepository.AddAsync(await _productRepository.GetUserAsync(User.Identity.Name), order);
                    return RedirectToAction("Complete");
                }
                else
                {
                    return RedirectToAction("Failed");
                }
            }
            OrderViewModel orderR = new OrderViewModel
            {
                Order = order,
                ShopCartItems = (List<ShopCartItem>)await _shopCart.GetAllAsync(new ShopCartSpecification().WhereUserEmail(User.Identity.Name))
            };

            return View(orderR);
        }

        public async Task<IActionResult> Complete()
        {
            await _shopCart.EmptyTheCart(User.Identity.Name);
            ViewBag.Message = "Заказ успешно обработан";
            return View();
        }

        public IActionResult Failed()
        {
            ViewBag.Message = "Заказ не был обработан";
            return View();
        }
    }
}
