using System;
using System.Linq.Expressions;
using WebApplication1.Data.Models;
using WebApplication1.Data.Specifications.Base;

namespace WebApplication1.Data.Specifications
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

        public ProductSpecification WhereInPriceRange(double min, double max)
        {
            if (min > max)
            {
                throw new ArgumentException("Min is greater than max!");
            }
            AddWhere(product => product.Price >= min && product.Price <= max);
            return this;
        }

        public ProductSpecification WhereId(int id)
        {
            AddWhere(product => product.Id == id);
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
        public ProductSpecification SortByFavourite()
        {
            AddDescendingOrdering(product => product.IsFavourite);
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
    }
}
