using B2DriverLicense.Core.EF;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace B2DriverLicense.Service.Repositories
{
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

        public bool SaveChange()
        {
            return _dbContext.SaveChanges() > 0;
        }

        public async Task<bool> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
