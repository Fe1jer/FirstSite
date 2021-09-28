using System.Collections.Generic;
using System.Threading.Tasks;
using InternetShop.Data.Models;
using InternetShop.Data.Specifications.Base;

namespace InternetShop.Data.Interfaces
{
    public interface IOrdersRepository
    {
        Task<IReadOnlyList<Order>> GetAllAsync();
        Task<IReadOnlyList<Order>> GetAllAsync(ISpecification<Order> specification);
        Task<Order> GetByIdAsync(int id);
        Task UpdateCourierOrdersAsync(int idOrder, User courier);
        Task DeleteAsync(Order order);
        Task AddAsync(string email, Order order);
        Task CompletedOrderAsync(Order order);
    }
}
