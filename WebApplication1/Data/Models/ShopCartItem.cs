namespace WebApplication1.Data.Models
{
    public class ShopCartItem
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public User User { get; set; }
        public int Price { get; set; }
        public string Category { set; get; }
        public string ShopCartId { get; set; }
    }
}
