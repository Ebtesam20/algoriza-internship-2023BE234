using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Core.Repository;
using Vezeeta.Core.Specifications;
using Vezeeta.Repository.Data;

namespace Vezeeta.Repository
{
    public class GenericRepository<T>:IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
                return await _context.Set<T>().ToListAsync();
        }
        public async Task<T> GetByIdAsync(int id)
          => await _context.Set<T>().FindAsync(id);

        public async Task<T> GetByIdStringAsync(string id)
          =>   await _context.Set<T>().FindAsync(id);
         
          

        public async Task<IReadOnlyList<T>> GetAllAsyncSpec(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();

        }

        public async Task<int> GetCountWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        public async Task<T> GetEntityAsyncSpec(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task AddAsync(T entity)
        => await _context.Set<T>().AddAsync(entity);

        public void Update(T entity)
       => _context.Set<T>().Update(entity);

        public void Delete(T entity)
         => _context.Set<T>().Remove(entity);


        //Private Function To apply specification
        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.QueryBuilder(_context.Set<T>(), spec);
        }

       
    }
}
