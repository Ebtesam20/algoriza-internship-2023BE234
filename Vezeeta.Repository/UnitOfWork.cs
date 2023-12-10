using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Core.Repository;
using Vezeeta.Repository.Data;

namespace Vezeeta.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbcontext;

        private Hashtable _repository;

        public UnitOfWork(AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task<int> Complete()
        {
            return await _dbcontext.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _dbcontext.DisposeAsync();
        }

        public IGenericRepository<Tentity>? Repository<Tentity>() where Tentity : class
        {
            if (_repository is null)
                _repository = new Hashtable();
            var type = typeof(Tentity).Name;
            if (!_repository.ContainsKey(type))
            {
                var repository = new GenericRepository<Tentity>(_dbcontext);
                _repository.Add(type, repository);

            }
            return _repository[type] as IGenericRepository<Tentity>;


        }
    }
}
