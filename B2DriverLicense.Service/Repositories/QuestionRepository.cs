using B2DriverLicense.Core.EF;
using B2DriverLicense.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace B2DriverLicense.Service.Repositories
{
    public interface IQuestionRepository : IRepositoryBase<Question>
    {
        IEnumerable<Question> GetQuestionsPaging(int page, int pageSize, bool include = false);
        Question GetQuestionByNumber(int number, bool include = false);
    }

    public class QuestionRepository : RepositoryBase<Question>, IQuestionRepository
    {
        public QuestionRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public Question GetQuestionByNumber(int number, bool include = false)
        {
            var question = _dbContext.Questions;
            if(include)
            {
                question.Include(x => x.Answers).Include(s => s.Hint);
            }

            var result = question.FirstOrDefault(x => x.Number == number);
            return result;
        }

        public IEnumerable<Question> GetQuestionsPaging(int page, int pageSize, bool include = false)
        {
            var question = _dbContext.Questions;
            if (include)
            {
                question.Include(x => x.Answers).Include(s => s.Hint);
            }

            var result = question.Skip((page - 1) * pageSize).Take(pageSize);
            return result.ToList();
        }
    }
}
