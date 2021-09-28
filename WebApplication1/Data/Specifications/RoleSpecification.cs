using System;
using System.Linq.Expressions;
using InternetShop.Data.Models;
using InternetShop.Data.Specifications.Base;

namespace InternetShop.Data.Specifications
{
    public class RoleSpecification : Specification<Role>
    {
        public RoleSpecification() : base() { }
        public RoleSpecification(int id) : this(role => role.Id == id) { }
        public RoleSpecification(string name) : this(role => role.Name.ToLower().Contains(name.ToLower())) { }
        public RoleSpecification(Expression<Func<Role, bool>> expression) : base(expression) { }
        public RoleSpecification SortByName()
        {
            AddOrdering(role => role.Name);
            return this;
        }
        public RoleSpecification WithoutTracking()
        {
            IsNoTracking = true;
            return this;
        }

        public RoleSpecification WithTracking()
        {
            IsNoTracking = false;
            return this;
        }
    }
}
