using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebApplication1.Data.AbstractClasses;
using WebApplication1.Data.Specifications.Base;

namespace WebApplication1.Data.Specifications
{
    internal static class SpecificationEvaluator
    {
        internal static IQueryable<T> ApplySpecification<T>(IQueryable<T> baseQuery, ISpecification<T> specification) where T : Entity
        {
            var query = baseQuery;

            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }

            query = specification.Includes.Aggregate
                (query, (current, include) => current.Include(include));

            var a = query.Expression.Type;
            if (specification.OrderByDescendingExpressions != null)
            {
                var count = specification.OrderByDescendingExpressions.Count;

                for (int i = 0; i < count; ++i)
                {
                    if (query is IOrderedQueryable<T> orderedQuery)
                    {
                        query = orderedQuery.ThenByDescending(specification.OrderByDescendingExpressions[i]);
                    }
                    else
                    {
                        query = query.OrderByDescending(specification.OrderByDescendingExpressions[i]);
                    }
                }
            }
            if (specification.OrderByExpressions != null)
            {
                var count = specification.OrderByExpressions.Count;

                for (int i = 0; i < count; ++i)
                {
                    if (query is IOrderedQueryable<T> orderedQuery)
                    {
                        query = orderedQuery.ThenBy(specification.OrderByExpressions[i]);
                    }
                    else
                    {
                        query = query.OrderBy(specification.OrderByExpressions[i]);
                    }
                }
            }

            query = specification.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));
            query = specification.WhereExpressions.Aggregate(query,
                (current, expression) => current.Where(expression));

            //Adding pagination
            if (specification.Skip > 0)
            {
                query = query.Skip(specification.Skip);
            }

            if (specification.Take > 0)
            {
                query = query.Take(specification.Take);
            }

            if (specification.IsNoTracking)
            {
                query.AsNoTracking();
            }

            return query;
        }
    }
}
