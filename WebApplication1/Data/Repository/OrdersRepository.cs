using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Models;
using WebApplication1.Data.Specifications;
using WebApplication1.Data.Specifications.Base;

namespace WebApplication1.Data.Repository
{
    public class OrdersRepository : Repository<Order>, IOrdersRepository
    {
        private readonly IShopCart shopCart;

        public OrdersRepository(AppDBContext appDBContext, IShopCart shopCart) : base(appDBContext)
        {
            this.shopCart = shopCart;
        }

        public async Task AddOrder(User user, Order order)
        {
            List<OrderDetail> orderDetails = new List<OrderDetail>();
            foreach (var el in await shopCart.GetShopItemsAsync(new ShopCartSpecification().IncludeProduct().WhereUser(user)))
            {
                var orderDetail = new OrderDetail()
                {
                    ProductId = el.Product.Id,
                    OrderId = order.Id,
                    Price = el.Product.Price
                };
                orderDetails.Add(orderDetail);
            }
            order.IdUser = user.Id;
            order.OrderDetails = orderDetails;
            order.Courier = null;
            order.OrderTime = DateTime.Now;
            order.IsRelevant = true;
            await AddAsync(order);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersAsync()
        {
            return await GetAllAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await GetByIdAsync(id);
        }

        public async Task CompletedOrderAsync(Order order)
        {
            order.IsRelevant = false;
            await UpdateAsync(order);
        }

        public async Task DeleteOrderAsync(Order order)
        {
            await DeleteAsync(order);
        }

        public async Task<IReadOnlyList<Order>> GetCourierOrdersAsync()
        {
            return await GetAllAsync();
        }

        public async Task UpdateCourierOrdersAsync(int idOrder, User courier)
        {
            var orders = await GetAllAsync(new OrderSpecification().IncludeCourier());
            Order order = orders.FirstOrDefault(n => n.Id == idOrder);
            order.Courier = courier;
            await UpdateAsync(order);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersAsync(ISpecification<Order> specification)
        {
            return await GetAllAsync(specification);
        }
    }
}
