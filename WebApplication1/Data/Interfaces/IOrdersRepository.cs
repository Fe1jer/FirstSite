using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Data.Models;
using WebApplication1.Data.Specifications.Base;

namespace WebApplication1.Data.Interfaces
{
    public interface IOrdersRepository
    {
        Task<IReadOnlyList<Order>> GetAllAsync();
        Task<IReadOnlyList<Order>> GetAllAsync(ISpecification<Order> specification);
        Task<Order> GetByIdAsync(int id);
        Task UpdateCourierOrdersAsync(int idOrder, User courier);
        Task DeleteAsync(Order order);
        Task AddAsync(User user, Order order);
        Task CompletedOrderAsync(Order order);
    }
}
