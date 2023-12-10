using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Vezeeta.Core.Specifications
{
    public class BaseSpecification<T> : ISpecification<T> where T : class
    {
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDesc { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
        public bool IsPaginationed { get; set; }
        public BaseSpecification()
        {

        }
        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        public void AddOrderby(Expression<Func<T, object>> orderBy)
        {
            OrderBy = orderBy;
        }

        public void AddOrderbyDesc(Expression<Func<T, object>> orderByDesc)
        {
            OrderByDesc = orderByDesc;
        }

        public void ApplyPagination(int skip, int take)
        {
            IsPaginationed = true;
            Skip = skip;
            Take = take;
        }
    }
}
