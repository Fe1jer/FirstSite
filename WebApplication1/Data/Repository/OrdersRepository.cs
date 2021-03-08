using System;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Models;

namespace WebApplication1.Data.Repository
{
    public class OrdersRepository : IAllOrders
    {
        private readonly AppDBContext appDBContext;
        private readonly ShopCart shopCart;

        public OrdersRepository(AppDBContext appDBContext, ShopCart shopCart)
        {
            this.appDBContext = appDBContext;
            this.shopCart = shopCart;
        }

        public void CreateOrder(Order order)
        {
            order.OrderTime = DateTime.Now;
            appDBContext.Order.Add(order);
            appDBContext.SaveChanges();

            var items = shopCart.ListShopItems;

            foreach (var el in items)
            {
                var orderDetail = new OrderDetail()
                {
                    ProductID = el.Product.Id,
                    OrderID = order.Id,
                    Prise = el.Product.Price
                };
                appDBContext.OrderDetail.Add(orderDetail);
            }
            appDBContext.SaveChanges();
        }
    }
}
