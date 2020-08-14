using B2DriverLicense.Core.EF;
using B2DriverLicense.Core.Entities;

namespace B2DriverLicense.Service.Repositories
{
    public interface IHintRepository : IRepositoryBase<Hint>
    {
    }

    public class HintRepository : RepositoryBase<Hint>, IHintRepository
    {
        public HintRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
