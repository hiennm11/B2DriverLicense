using B2DriverLicense.Core.EF;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace B2DriverLicense.Service
{
    public interface IUnitOfWork : IDisposable
    {
        public int SaveChange();
        public Task<int> SaveChangeAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private bool disposed = false;

        public UnitOfWork(AppDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public int SaveChange()
        {
            return _dbContext.SaveChanges();
        }

        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
