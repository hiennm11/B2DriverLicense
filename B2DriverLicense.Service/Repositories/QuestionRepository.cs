using B2DriverLicense.Core.Dtos;
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
        IEnumerable<Question> GetQuestionsPaging(int page, int pageSize, bool include = false, int chapterId = 0);
        Question GetQuestionByNumber(int number, bool include = false);
        Question GetQuestionById(int id, bool include = false);

    }

    public class QuestionRepository : RepositoryBase<Question>, IQuestionRepository
    {
        public QuestionRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
                       
        public Question GetQuestionById(int id, bool include = false)
        {
            var result = _dbContext.Questions.FirstOrDefault(x => x.Id == id);

            if (result == null)
            {
                return null;
            }

            if (include)
            {
                _dbContext.Entry(result).Reference(x => x.Chapter).Load();
                _dbContext.Entry(result).Collection(x => x.Answers).Load();
            }

            return result;
        }

        public Question GetQuestionByNumber(int number, bool include = false)
        {            
            var result = _dbContext.Questions.FirstOrDefault(x => x.Number == number);

            if (result == null)
            {
                return null;
            }

            if (include)
            {
                _dbContext.Entry(result).Reference(x => x.Chapter).Load();
                _dbContext.Entry(result).Collection(x => x.Answers).Load();
            }

            return result;
        }

        public IEnumerable<Question> GetQuestionsPaging(int page, int pageSize, bool include = false, int chapterId = 0)
        {
            var question = _dbContext.Questions.AsQueryable();

            if (chapterId > 0)
            {
                question = question.Where(x => x.ChapterId == chapterId);
            }

            if (include)
            {
                question = question.Include(x => x.Answers);
            }

            var result = question.Skip((page - 1) * pageSize).Take(pageSize);

            return result;
        }
        
    }
}
