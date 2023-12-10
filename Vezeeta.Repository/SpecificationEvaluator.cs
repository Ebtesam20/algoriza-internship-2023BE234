using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Core.Specifications;

namespace Vezeeta.Repository
{
    public static class SpecificationEvaluator<TEntity> where TEntity : class
    {
        public static IQueryable<TEntity> QueryBuilder(IQueryable<TEntity> InputQuery, ISpecification<TEntity> Spec)
        {
            var Query = InputQuery;
            if (Spec.Criteria is not null)
                Query = Query.Where(Spec.Criteria);

            if (Spec.OrderBy is not null)
                Query = Query.OrderBy(Spec.OrderBy);

            if (Spec.OrderByDesc is not null)
                Query = Query.OrderByDescending(Spec.OrderByDesc);

            if (Spec.IsPaginationed)
                Query = Query.Skip(Spec.Skip).Take(Spec.Take);


            Query = Spec.Includes.Aggregate(Query, (CurrentQuery, IncludeExp) => CurrentQuery.Include(IncludeExp));
            return Query;
        }
    }
}
