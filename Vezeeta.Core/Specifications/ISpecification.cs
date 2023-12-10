using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Vezeeta.Core.Specifications
{
    public interface ISpecification<T> where T : class
    {
        public Expression<Func<T, bool>> Criteria { get; set; } //for where expression

        public List<Expression<Func<T, object>>> Includes { get; set; } //for include

        public Expression<Func<T, object>> OrderBy { get; set; }

        public Expression<Func<T, object>> OrderByDesc { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
        public bool IsPaginationed { get; set; }
    }
}
