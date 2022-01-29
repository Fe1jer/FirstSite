using System;
using System.Linq.Expressions;
using InternetShop.Data.Models;
using InternetShop.Data.Specifications.Base;

namespace InternetShop.Data.Specifications
{
    public class ShopCartSpecification : Specification<ShopCartItem>
    {
        public ShopCartSpecification() : base()
        {
            AddInclude(shopCart => shopCart.Product);
            AddInclude(shopCart => shopCart.Product.ProductType);
        }
        public ShopCartSpecification(Expression<Func<ShopCartItem, bool>> expression) : base(expression)
        {
            AddInclude(shopCart => shopCart.Product);
            AddInclude(shopCart => shopCart.Product.ProductType);
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
