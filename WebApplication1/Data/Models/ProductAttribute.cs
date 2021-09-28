using InternetShop.Data.AbstractClasses;

namespace InternetShop.Data.Models
{
    public class ProductAttribute : Entity
    {
        public string Value { get; set; }
        public int ProductId { get; set; }
        public AttributeCategory AttributeCategory { get; set; }
    }
}
