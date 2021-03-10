using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApplication1.Data.Models;
using WebApplication1.Data.Specifications.Base;

namespace WebApplication1.Data.Specifications
{
    public class ShopCartSpecification : Specification<ShopCartItem>
    {
        public ShopCartSpecification() : base() { }
        public ShopCartSpecification(Expression<Func<ShopCartItem, bool>> expression) : base(expression) { }

        public ShopCartSpecification WhereProduct(int id)
        {
            AddWhere(shopCart => shopCart.Product.Id == id);
            return this;
        }

        public ShopCartSpecification WhereProduct(Product Product)
        {
            AddWhere(shopCart => shopCart.Product == Product);
            return this;
        }

        public ShopCartSpecification WhereUser(User user)
        {
            AddWhere(shopCart => shopCart.User == user);
            return this;
        }

        public ShopCartSpecification SortByProduct()
        {
            AddOrdering(shopCart => shopCart.Product);
            return this;
        }

        public ShopCartSpecification IncludeProduct()
        {
            AddInclude(shopCart => shopCart.Product);
            return this;
        }
        public ShopCartSpecification WithoutTracking()
        {
            IsNoTracking = true;
            return this;
        }

        public ShopCartSpecification WithTracking()
        {
            IsNoTracking = false;
            return this;
        }
    }
}
