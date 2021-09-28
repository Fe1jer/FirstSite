using System.Collections.Generic;
using InternetShop.Data.Models;

namespace InternetShop.ViewModels
{
    public class ChangeCourierViewModels
    {
        public Order Order { get; set; }
        public IEnumerable<User> AllCouriers { get; set; }
    }
}
