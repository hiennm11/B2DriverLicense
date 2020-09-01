using B2DriverLicense.Core.EF;
using Microsoft.EntityFrameworkCore;
using System;
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
        public void Update(T entity);
        public void Delete(T entity);

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
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbContext.Set<T>().Add(entity);
        }

        public async Task AddAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await _dbContext.Set<T>().AddAsync(entity);
        }

        public void AddRange(List<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }
            _dbContext.Set<T>().AddRange(entities);
        }

        public async Task AddRangeAsync(List<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }
            await _dbContext.Set<T>().AddRangeAsync(entities);
        }

        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _dbContext.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbContext.Update(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }       
    }
}
