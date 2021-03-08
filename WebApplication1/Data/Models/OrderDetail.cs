using WebApplication1.Data.AbstractClasses;

namespace WebApplication1.Data.Models
{
    public class OrderDetail : Entity
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public uint Prise { get; set; }
        public virtual Product Product { get; set; }
        public virtual Order Order { get; set; }
    }
}
