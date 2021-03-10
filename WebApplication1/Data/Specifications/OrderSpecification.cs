﻿using WebApplication1.Data.Models;
using WebApplication1.Data.Specifications.Base;
using System;
using System.Linq.Expressions;

namespace WebApplication1.Data.Specifications
{
    public class OrderSpecification : Specification<Order>
    {
        public OrderSpecification() : base() { }

        public OrderSpecification(int id) : this(order => order.Id == id) { }

        public OrderSpecification(string address) : this(order => order.Address.ToLower().Contains(address.ToLower())) { }

        public OrderSpecification(Expression<Func<Order, bool>> expression) : base(expression) { }

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
        public OrderSpecification IncludeDetails(string includeString)
        {
            AddInclude(includeString);
            return this;
        }
        public OrderSpecification IncludeDetails()
        {
            AddInclude("OrderDetails.Product");
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