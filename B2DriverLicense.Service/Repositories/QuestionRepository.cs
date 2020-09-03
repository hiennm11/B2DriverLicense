using B2DriverLicense.Core.Dtos;
using B2DriverLicense.Core.EF;
using B2DriverLicense.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2DriverLicense.Service.Repositories
{
    public interface IQuestionRepository : IRepositoryBase<Question>
    {        
        Task<IEnumerable<Question>> GetQuestionsPagingAsync(int page, int pageSize, bool include = false, int chapterId = 0);
        Task<Question> GetQuestionByNumberAsync(int number, bool include = false);
        Task<IEnumerable<Answer>> GetAnswersByQuestionNumberAsync(int number);
        Task<Answer> GetAnswerAsync(int number, int key);
        public void UpdateAnswer(Answer entity);
        public void DeleteAnswer(Answer entity);
        Task<Hint> GetHintAsync(int number);
        public void UpdateHint(Hint entity);
        public void DeleteHint(Hint entity);
    }

    public class QuestionRepository : RepositoryBase<Question>, IQuestionRepository
    {
        public QuestionRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public void DeleteAnswer(Answer entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _dbContext.Answers.Remove(entity);
        }

        public void DeleteHint(Hint entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _dbContext.Hints.Remove(entity);
        }

        public async Task<Answer> GetAnswerAsync(int number, int key)
        {
            var question = await _dbContext.Questions.FirstOrDefaultAsync(x => x.Number == number);

            if (question == null)
            {
                throw new ArgumentNullException(nameof(question));
            }

            var list = _dbContext.Answers.Where(x => x.QuestionId == question.Id);

            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            return await list.FirstOrDefaultAsync(s => s.Key == key);
        }

        public async Task<IEnumerable<Answer>> GetAnswersByQuestionNumberAsync(int number)
        {
            var question = await _dbContext.Questions.FirstOrDefaultAsync(x => x.Number == number);

            if (question == null) return null;

            return await _dbContext.Answers.Where(s => s.QuestionId == question.Id).ToListAsync();
        }

        public async Task<Hint> GetHintAsync(int number)
        {
            var question = await _dbContext.Questions.FirstOrDefaultAsync(x => x.Number == number);

            if (question == null) return null;

            return await _dbContext.Hints.FirstOrDefaultAsync(x => x.QuestionId == question.Id);
        }

        public async Task<Question> GetQuestionByNumberAsync(int number, bool include = false)
        {            
            var result = await _dbContext.Questions.FirstOrDefaultAsync(x => x.Number == number);

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

        public async Task<IEnumerable<Question>> GetQuestionsPagingAsync(int page, int pageSize, bool include = false, int chapterId = 0)
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

            return await result.ToListAsync();
        }

        public void UpdateAnswer(Answer entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbContext.Update(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void UpdateHint(Hint entity)
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
