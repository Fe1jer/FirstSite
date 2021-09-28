using InternetShop.Data.AbstractClasses;

namespace InternetShop.Data.Models
{
    public class OrderDetail : Entity
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public uint Price { get; set; }
        public virtual Product Product { get; set; }
        public virtual Order Order { get; set; }
    }
}
