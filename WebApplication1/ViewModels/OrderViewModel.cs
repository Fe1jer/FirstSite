using WebApplication1.Data.Models;
using System.Collections.Generic;

namespace WebApplication1.ViewModels
{
    public class OrderViewModel
    {
        public Order Order { get; set; }
        public Dictionary<Order, List<OrderDetail>> AllOrders { get; set; }
        public ShopCart ShopCart { get; set; }
    }
}
