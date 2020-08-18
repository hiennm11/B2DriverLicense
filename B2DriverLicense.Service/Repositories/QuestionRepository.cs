using B2DriverLicense.Core.EF;
using B2DriverLicense.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace B2DriverLicense.Service.Repositories
{
    public interface IQuestionRepository : IRepositoryBase<Question>
    {
        Question GetQuestionByNumber(int number);
    }

    public class QuestionRepository : RepositoryBase<Question>, IQuestionRepository
    {
        public QuestionRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public Question GetQuestionByNumber(int number)
        {
            return _dbContext.Questions.FirstOrDefault(x => x.Number == number);
        }
    }
}
