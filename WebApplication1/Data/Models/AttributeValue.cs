using InternetShop.Data.AbstractClasses;

namespace InternetShop.Data.Models
{
    public class AttributeValue : Entity
    {
        public string Value { get; set; }
        public int ProductId { get; set; }
        public Attribute Attribute { get; set; }
    }
}
