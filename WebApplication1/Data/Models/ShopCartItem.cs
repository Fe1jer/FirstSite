using WebApplication1.Data.AbstractClasses;

namespace WebApplication1.Data.Models
{
    public class ShopCartItem : Entity
    {
        public Product Product { get; set; }
        public User User { get; set; }
        public int Price { get; set; }
        public string Category { set; get; }
    }
}
