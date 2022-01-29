using InternetShop.Data.Models;
using InternetShop.Data.Specifications.Base;

namespace InternetShop.Data.Specifications
{
    public class AttributeValuesSpecification : Specification<AttributeValue>
    {
        public AttributeValuesSpecification() : base() { }

        public AttributeValuesSpecification WhereProductId(int productId)
        {
            AddWhere(p => p.ProductId == productId);
            AddInclude(p => p.Attribute);
            return this;
        }

        public AttributeValuesSpecification WithoutTracking()
        {
            IsNoTracking = true;
            return this;
        }

        public AttributeValuesSpecification WithTracking()
        {
            IsNoTracking = false;
            return this;
        }
    }
}
