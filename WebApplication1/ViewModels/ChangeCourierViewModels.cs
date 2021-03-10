using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data.Models;

namespace WebApplication1.ViewModels
{
    public class ChangeCourierViewModels
    {
        public Order Order { get; set; }
        public IEnumerable<User> AllCouriers { get; set; }
    }
}
