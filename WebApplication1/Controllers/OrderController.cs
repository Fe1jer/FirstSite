using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetShop.Data.Interfaces;
using InternetShop.Data.Models;
using InternetShop.Data.Specifications;
using InternetShop.ViewModels;

namespace InternetShop.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly IShopCart _shopCart;
        private readonly IProductRepository _productRepository;

        public OrderController(IShopCart shopCart, IOrdersRepository IOrdersRepository, IProductRepository productRepository)
        {
            _shopCart = shopCart;
            _ordersRepository = IOrdersRepository;
            _productRepository = productRepository;
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
                if (countShopCartItems > 0)
                {
                    await _ordersRepository.AddAsync(User.Identity.Name, order);
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
                ShopCartItems = _shopCart.GetAllAsync(new ShopCartSpecification().WhereUserEmail(User.Identity.Name)).GetAwaiter().GetResult().ToList()
            };

            return View(orderR);
        }

        public async Task<IActionResult> Complete()
        {
            var shopCartItems = await _shopCart.GetAllAsync(new ShopCartSpecification().WhereUserEmail(User.Identity.Name));
            await _productRepository.BuyGoods(shopCartItems.Select(p => p.Product).ToList());
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
