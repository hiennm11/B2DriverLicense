using B2DriverLicense.Core.EF;
using System.Threading.Tasks;

namespace B2DriverLicense.Service.Repositories
{
    public interface IRepositoryBase<T> where T : class
    {
        public void Add(T entity);
        public Task AddAsync(T entity);
    }

    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly AppDbContext _dbContext;

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
    }
}
