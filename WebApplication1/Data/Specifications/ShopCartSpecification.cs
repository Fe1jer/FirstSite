using System;
using System.Linq.Expressions;
using WebApplication1.Data.Models;
using WebApplication1.Data.Specifications.Base;

namespace WebApplication1.Data.Specifications
{
    public class ShopCartSpecification : Specification<ShopCartItem>
    {
        public ShopCartSpecification() : base()
        {
            AddInclude(shopCart => shopCart.Product);
        }
        public ShopCartSpecification(Expression<Func<ShopCartItem, bool>> expression) : base(expression)
        {
            AddInclude(shopCart => shopCart.Product);
        }

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

        public ShopCartSpecification WhereUserEmail(string email)
        {
            AddInclude(shopCart => shopCart.User);
            AddWhere(shopCart => shopCart.User.Email == email);
            return this;
        }

        public ShopCartSpecification SortByProduct()
        {
            AddOrdering(shopCart => shopCart.Product);
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
