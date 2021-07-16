using WebApplication1.Data.Models;
using WebApplication1.Data.Specifications.Base;

namespace WebApplication1.Data.Specifications
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
