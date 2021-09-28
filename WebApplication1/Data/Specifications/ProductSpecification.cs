using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using InternetShop.Data.Models;
using InternetShop.Data.Specifications.Base;

namespace InternetShop.Data.Specifications
{
    public class ProductSpecification : Specification<Product>
    {
        public ProductSpecification() : base() { }
        public ProductSpecification(Expression<Func<Product, bool>> expression) : base(expression) { }

        public ProductSpecification SortByName()
        {
            AddDescendingOrdering(product => product.Name);
            return this;
        }

        public ProductSpecification SortByRelevance()
        {
            AddDescendingOrdering(product => product.Available);
            AddDescendingOrdering(product => product.IsFavourite);
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

        public ProductSpecification WhereAvailable(bool isAvailable)
        {
            AddWhere(product => product.Available == isAvailable);
            return this;
        }
        public ProductSpecification WhereName(string name)
        {
            AddWhere(product => product.Name.ToLower().Contains(name.ToLower()));
            return this;
        }
        public ProductSpecification SortByPrice()
        {
            AddDescendingOrdering(product => product.Price);
            return this;
        }

        public ProductSpecification IncludeAttribute()
        {
            AddInclude("ProductAttributes.AttributeCategory");
            return this;
        }
        public ProductSpecification IncludeCategory()
        {
            AddInclude(product => product.Category);
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
