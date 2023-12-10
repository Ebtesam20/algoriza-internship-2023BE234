using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Core.Specifications;

namespace Vezeeta.Core.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> GetByIdStringAsync(string id);

        Task<IReadOnlyList<T>> GetAllAsyncSpec(ISpecification<T> spec);
        Task<int> GetCountWithSpecAsync(ISpecification<T> spec);
        Task<T> GetEntityAsyncSpec(ISpecification<T> spec);

        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        
    }
}
