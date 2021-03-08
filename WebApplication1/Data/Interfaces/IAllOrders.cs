using System.Collections.Generic;
using WebApplication1.Data.Models;

namespace WebApplication1.Data.Interfaces
{
    public interface IAllOrders
    {
        public Order GetOrder(int id);
        public List<Order> GetCourierOrders(string courier);
        public void SetCourierOrders(int idOrder, int idCourier);
        public void DeleteOrder(int id);
        public List<Order> GetAllOrders();
        void CreateOrder(User user ,Order order);
    }
}
