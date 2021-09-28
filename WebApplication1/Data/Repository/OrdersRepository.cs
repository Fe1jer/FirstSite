using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetShop.Data.Interfaces;
using InternetShop.Data.Models;
using InternetShop.Data.Specifications;
using InternetShop.Data.Specifications.Base;

namespace InternetShop.Data.Repository
{
    public class OrdersRepository : Repository<Order>, IOrdersRepository
    {
        private readonly IShopCart shopCart;
        private readonly IUserRepository _userRepository;

        public OrdersRepository(AppDBContext appDBContext, IShopCart shopCart, IUserRepository userRepository) : base(appDBContext)
        {
            _userRepository = userRepository;
            this.shopCart = shopCart;
        }

        public async Task AddAsync(string email, Order order)
        {
            User user = await _userRepository.GetUserAsync(email);
            List<OrderDetail> orderDetails = new List<OrderDetail>();
            foreach (var el in await shopCart.GetAllAsync(new ShopCartSpecification().WhereUserEmail(user.Email)))
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

        public new async Task<IReadOnlyList<Order>> GetAllAsync()
        {
            return await base.GetAllAsync();
        }

        public new async Task<Order> GetByIdAsync(int id)
        {
            return await base.GetByIdAsync(id);
        }

        public async Task CompletedOrderAsync(Order order)
        {
            order.IsRelevant = false;
            await UpdateAsync(order);
        }

        public new async Task DeleteAsync(Order order)
        {
            await base.DeleteAsync(order);
        }

        public async Task UpdateCourierOrdersAsync(int idOrder, User courier)
        {
            var orders = await base.GetAllAsync(new OrderSpecification());
            Order order = orders.FirstOrDefault(n => n.Id == idOrder);
            order.Courier = courier;
            await UpdateAsync(order);
        }

        public new async Task<IReadOnlyList<Order>> GetAllAsync(ISpecification<Order> specification)
        {
            return await base.GetAllAsync(specification);
        }
    }
}
