using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using InternetShop.Data.Models;
using InternetShop.Data.Specifications.Base;

namespace InternetShop.Data.Specifications
{
    public class ProductSpecification : Specification<Product>
    {
        public ProductSpecification() : base() {
            IncludeAttribute();
            IncludeType();
        }
        public ProductSpecification(Expression<Func<Product, bool>> expression) : base(expression) {
            IncludeAttribute();
            IncludeType();
        }

        public ProductSpecification SortByName()
        {
            AddDescendingOrdering(product => product.ProductType.Name);
            return this;
        }

        public ProductSpecification SortByRelevance()
        {
            AddDescendingOrdering(product => product.Id);
            return this;
        }

        public ProductSpecification WhereInPriceRange(double min, double max)
        {
            if (min > max)
            {
                throw new ArgumentException("Min is greater than max!");
            }
            AddWhere(product => product.Price >= min && product.Price <= max);
            return this;
        }

        public ProductSpecification WhereNotOnTheList(List<Product> list)
        {
            AddWhere(Product => !list.Contains(Product));
            return this;
        }

        public ProductSpecification WhereName(string name)
        {
            AddWhere(product => product.ProductType.Name.ToLower().Contains(name.ToLower()));
            return this;
        }
        public ProductSpecification SortByPrice()
        {
            AddDescendingOrdering(product => product.Price);
            return this;
        }

        public ProductSpecification IncludeAttribute()
        {
            AddInclude(p => p.AttributeValues);
            AddInclude("AttributeValues.Attribute");

            return this;
        }
        public ProductSpecification IncludeType()
        {
            AddInclude(product => product.ProductType);
            return this;
        }
        public ProductSpecification WithoutTracking()
        {
            IsNoTracking = true;
            return this;
        }

        public ProductSpecification WithTracking()
        {
            IsNoTracking = false;
            return this;
        }

        public new ProductSpecification Take(int count)
        {
            base.Take = count;
            return this;
        }
    }
}
