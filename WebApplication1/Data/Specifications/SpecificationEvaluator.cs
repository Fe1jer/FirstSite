using WebApplication1.Data.AbstractClasses;
using WebApplication1.Data.Specifications.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace WebApplication1.Data.Specifications
{
    internal static class SpecificationEvaluator
	{

		internal static IQueryable<T> ApplySpecification<T>(IQueryable<T> baseQuery, ISpecification<T> specification) where T : Entity
		{
			var query = baseQuery;

			if (specification.Criteria != null) query = query.Where(specification.Criteria);

			query = specification.Includes.Aggregate
				(query, (current, include) => current.Include(include));

			query = specification.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));

			query = specification.OrderByExpressions.Aggregate(query,
				(current, expression) => current.OrderBy(expression));

			query = specification.OrderByDescendingExpressions.Aggregate(query,
				(current, expression) => current.OrderByDescending(expression));

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
