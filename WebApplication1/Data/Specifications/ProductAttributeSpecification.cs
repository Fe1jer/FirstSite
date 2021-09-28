using InternetShop.Data.Models;
using InternetShop.Data.Specifications.Base;

namespace InternetShop.Data.Specifications
{
    public class ProductAttributeSpecification : Specification<ProductAttribute>
    {
        public ProductAttributeSpecification() : base() { }

        public ProductAttributeSpecification WhereProductId(int productId)
        {
            AddWhere(p => p.ProductId == productId);
            AddInclude(p => p.AttributeCategory);
            return this;
        }

        public ProductAttributeSpecification WithoutTracking()
        {
            IsNoTracking = true;
            return this;
        }

        public ProductAttributeSpecification WithTracking()
        {
            IsNoTracking = false;
            return this;
        }
    }
}
