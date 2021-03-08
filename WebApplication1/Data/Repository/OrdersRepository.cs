using System;
using System.Collections.Generic;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace WebApplication1.Data.Repository
{
    public class OrdersRepository : IAllOrders
    {
        private readonly AppDBContext appDBContext;
        private readonly IShopCart shopCart;

        public OrdersRepository(AppDBContext appDBContext, IShopCart shopCart)
        {
            this.appDBContext = appDBContext;
            this.shopCart = shopCart;
        }

        public void CreateOrder(User user ,Order order)
        {
            order.Courier = null;
            order.OrderTime = DateTime.Now;
            appDBContext.Order.Add(order);
            appDBContext.SaveChanges();

            foreach (var el in shopCart.GetShopItems(user))
            {
                var orderDetail = new OrderDetail()
                {
                    ProductId = el.Product.Id,
                    OrderId = order.Id,
                    Prise = el.Product.Price
                };
                appDBContext.OrderDetail.Add(orderDetail);
            }
            appDBContext.SaveChanges();
        }

        public List<Order> GetAllOrders()
        {
            List<Order> allOrders = new List<Order>();
            foreach (Order order in appDBContext.Order.Include(p => p.Courier).OrderBy(p => p.Courier).ToList())
            {
                allOrders.Add(order);
                order.OrderDetails = appDBContext.OrderDetail.Include(p => p.Product).Where(p => p.OrderId == order.Id).ToList();
            }
            return allOrders;
        }

        public Order GetOrder(int id)
        {
            Dictionary<Order, List<OrderDetail>> getOrder = new Dictionary<Order, List<OrderDetail>>();
            Order order = appDBContext.Order.Include(p => p.Courier).Where(p => p.Id == id).FirstOrDefault();
            order.OrderDetails = appDBContext.OrderDetail.Include(p => p.Product).Where(p => p.OrderId == order.Id).ToList();
            return order;
        }

        public void DeleteOrder(int id)
        {
            Dictionary<Order, List<OrderDetail>> getOrder = new Dictionary<Order, List<OrderDetail>>();
            Order order = appDBContext.Order.Where(p => p.Id == id).FirstOrDefault();
            List<OrderDetail> ordersDetail = appDBContext.OrderDetail.Include(p => p.Product).Where(p => p.OrderId == order.Id).ToList();
            appDBContext.Order.Remove(order);
            foreach (OrderDetail orderDetail in ordersDetail)
            {
                appDBContext.OrderDetail.Remove(orderDetail);
            }
            appDBContext.SaveChanges();
        }
        public List<Order> GetCourierOrders(string courier)
        {
            List<Order> allOrders = appDBContext.Order.Include(p => p.Courier).Where(p => p.Courier.Email == courier).ToList();
            foreach (Order order in allOrders)
            {
                order.OrderDetails = appDBContext.OrderDetail.Include(p => p.Product).Where(p => p.OrderId == order.Id).ToList();
            }
            return allOrders;
        }
        public void SetCourierOrders(int idOrder, int idCourier)
        {
            Order order = appDBContext.Order.Include(p => p.Courier).Where(p => p.Id == idOrder).FirstOrDefault();
            User user = appDBContext.Users.Where(p => p.Id == idCourier).FirstOrDefault();
            if (idCourier != 0)
            {
                order.Courier = user;
            }
            else
            {
                order.Courier = null;
            }
            
            appDBContext.SaveChanges();
        }
    }
}
