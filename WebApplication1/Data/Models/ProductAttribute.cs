using WebApplication1.Data.AbstractClasses;

namespace WebApplication1.Data.Models
{
    public class ProductAttribute : Entity
    {
        public string Value { get; set; }
        public int ProductId { get; set; }
        public AttributeCategory AttributeCategory { get; set; }
    }
}
