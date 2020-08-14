using B2DriverLicense.Core.EF;
using B2DriverLicense.Core.Entities;

namespace B2DriverLicense.Service.Repositories
{
    public interface IAnswerRepository : IRepositoryBase<Answer>
    {
    }

    public class AnswerRepository : RepositoryBase<Answer>, IAnswerRepository
    {
        public AnswerRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
