using InternetShop.Data.AbstractClasses;

namespace InternetShop.Data.Models
{
    public class ShopCartItem : Entity
    {
        public Product Product { get; set; }
        public User User { get; set; }
        public int Price { get; set; }
        public string Category { set; get; }
    }
}
