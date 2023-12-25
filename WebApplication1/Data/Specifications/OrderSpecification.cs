using System;
using System.Linq;
using System.Linq.Expressions;
using InternetShop.Data.Models;
using InternetShop.Data.Specifications.Base;

namespace InternetShop.Data.Specifications
{
    public class OrderSpecification : Specification<Order>
    {
        public OrderSpecification() : base()
        {
            IncludeDetails();
        }

        public OrderSpecification(int id) : this(order => order.Id == id) { }

        public OrderSpecification(string address) : this(order => order.Address.ToLower().Contains(address.ToLower())) { }

        public OrderSpecification(Expression<Func<Order, bool>> expression) : base(expression)
        {
            IncludeDetails();
        }

        public OrderSpecification WhereName(string name)
        {
            AddWhere(order => order.Name.ToLower().Contains(name.ToLower()));
            return this;
        }

        public OrderSpecification WhereId(int id)
        {
            AddWhere(order => order.Id == id);
            return this;
        }

        public OrderSpecification WhereCourierEmail(string email)
        {
            AddWhere(order => order.Courier.Email.Contains(email));
            return this;
        }

        public OrderSpecification WhereActual()
        {
            AddWhere(order => order.IsRelevant == true);
            return this;
        }

        public OrderSpecification SortByDate()
        {
            AddDescendingOrdering(order => order.OrderTime);
            return this;
        }

        public OrderSpecification SortByCourier()
        {
            AddOrdering(order => order.Courier);
            return this;
        }

        public OrderSpecification IncludeCourier()
        {
            AddInclude(order => order.Courier);
            return this;
        }

        private OrderSpecification IncludeDetails()
        {
           AddInclude("OrderDetails.Product.ProductType");
            return this;
        }

        public OrderSpecification WithoutTracking()
        {
            IsNoTracking = true;
            return this;
        }

        public OrderSpecification WithTracking()
        {
            IsNoTracking = false;
            return this;
        }
    }
}
