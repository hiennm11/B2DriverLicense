using System;
using System.Threading.Tasks;

namespace B2DriverLicense.Service.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        public bool SaveChange();
        public Task<bool> SaveChangeAsync();
    }
}
