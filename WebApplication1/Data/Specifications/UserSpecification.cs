using System;
using System.Linq.Expressions;
using InternetShop.Data.Models;
using InternetShop.Data.Specifications.Base;

namespace InternetShop.Data.Specifications
{
    public class UserSpecification : Specification<User>
    {
        public UserSpecification() : base() { }
        public UserSpecification(int id) : this(user => user.Id == id) { }
        public UserSpecification(string name) : this(user => user.Email.ToLower().Contains(name.ToLower())) { }
        public UserSpecification(Expression<Func<User, bool>> expression) : base(expression) { }
        public UserSpecification SortByRole()
        {
            AddDescendingOrdering(user => user.Role);
            AddOrdering(user => user.Email);
            return this;
        }
        public UserSpecification IncludeRole()
        {
            AddInclude(user => user.Role);
            return this;
        }
        public UserSpecification WithoutTracking()
        {
            IsNoTracking = true;
            return this;
        }

        public UserSpecification WhereEmail(string email)
        {
            if (email != null)
            {
                AddWhere(user => user.Email.ToLower().Contains(email.ToLower()));
            }
            return this;
        }

        public UserSpecification WhereRole(string role)
        {
            AddWhere(p => p.Role.Name == role);
            return this;
        }

        public UserSpecification WithTracking()
        {
            IsNoTracking = false;
            return this;
        }
    }
}
