using B2DriverLicense.Core.EF;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace B2DriverLicense.Service.Repositories
{
    public interface IRepositoryBase<T> where T : class
    {
        public void Add(T entity);
        public Task AddAsync(T entity);
        public void AddRange(List<T> entities);
        public Task AddRangeAsync(List<T> entities);
    }

    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected readonly AppDbContext _dbContext;

        public RepositoryBase(AppDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public void Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
        }

        public async Task AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }

        public void AddRange(List<T> entities)
        {
            _dbContext.Set<T>().AddRange(entities);
        }

        public async Task AddRangeAsync(List<T> entities)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities);
        }
    }
}
