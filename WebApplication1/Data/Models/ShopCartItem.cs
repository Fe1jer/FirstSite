using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Data.Models
{
    public class ShopCartItem
    {
        public int Id { get; set; }
        public Car Car { get; set; }
        public int Price { get; set; }

        public Category Category { set; get; }
        public string ShopCartId { get; set; }
    }
}
