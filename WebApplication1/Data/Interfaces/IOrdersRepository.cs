using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Data.Models;
using WebApplication1.Data.Specifications.Base;

namespace WebApplication1.Data.Interfaces
{
    public interface IOrdersRepository
    {
        Task<IReadOnlyList<Order>> GetOrdersAsync();
        Task<IReadOnlyList<Order>> GetOrdersAsync(ISpecification<Order> specification);
        Task<Order> GetOrderByIdAsync(int id);
        Task UpdateCourierOrdersAsync(int idOrder, User courier);
        Task DeleteOrderAsync(Order order);
        Task AddOrder(User user, Order order);
    }
}
