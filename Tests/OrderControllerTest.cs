using InternetShop.Controllers;
using InternetShop.Data.Interfaces;
using InternetShop.Data.Models;
using InternetShop.Data.Specifications.Base;
using InternetShop.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class OrderControllerTest
    {
        OrderController OrderController { get; set; }

        public OrderControllerTest()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "anuviswan"),
            }, "mock"));
            var mockShopCart = new Mock<IShopCart>();
            mockShopCart.Setup(x => x.GetAllAsync(It.IsAny<ISpecification<ShopCartItem>>())).ReturnsAsync(new List<ShopCartItem>() { new ShopCartItem() });
            var mockOrdersRepository = new Mock<IOrdersRepository>();
            var mockProductRepository = new Mock<IProductRepository>();
            OrderController = new OrderController(mockShopCart.Object, mockOrdersRepository.Object, mockProductRepository.Object);
            OrderController.ControllerContext.HttpContext = new DefaultHttpContext() { User = user };
        }

        [Fact]
        public async Task CheckoutReturnsViewResultWithOrderModelAsync()
        {
            // Arrange
            OrderController.ModelState.AddModelError("", "Не все поля введены");
            Order newOrder = new Order();
            // Act
            OrderViewModel orderR = new OrderViewModel
            {
                Order = newOrder,
                ShopCartItems = new List<ShopCartItem> { new ShopCartItem() }
            };
            var result = await OrderController.Checkout(newOrder, 0);
            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            orderR = (OrderViewModel)viewResult.Model;
            Assert.Equal(orderR, ((OrderViewModel)viewResult.Model));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(10)]
        [InlineData(-10)]
        public async Task CheckoutReturnsViewResultRedirectToAction(int countShopCartItems)
        {
            // Arrange
            Order newOrder = new Order();
            var result = await OrderController.Checkout(newOrder, countShopCartItems);
            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            if (countShopCartItems > 0)
            {
                Assert.Equal("Complete", viewResult.ActionName);
            }
            else
            {
                Assert.Equal("Failed", viewResult.ActionName);
            }
        }
    }
}
