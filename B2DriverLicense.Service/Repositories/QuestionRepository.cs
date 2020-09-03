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
        IEnumerable<Answer> GetAnswersByQuestionNumber(int number);
        Answer GetAnswer(int number, int key);
        public void UpdateAnswer(Answer entity);
        public void DeleteAnswer(Answer entity);
        Hint GetHint(int number);
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

        public Answer GetAnswer(int number, int key)
        {
            var question = _dbContext.Questions.FirstOrDefault(x => x.Number == number);

            if (question == null)
            {
                throw new ArgumentNullException(nameof(question));
            }

            var list = _dbContext.Answers.Where(x => x.QuestionId == question.Id);

            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            return list.FirstOrDefault(s => s.Key == key);
        }

        public IEnumerable<Answer> GetAnswersByQuestionNumber(int number)
        {
            var question = _dbContext.Questions.FirstOrDefault(x => x.Number == number);

            if (question == null) return null;

            return _dbContext.Answers.Where(s => s.QuestionId == question.Id);
        }

        public Hint GetHint(int number)
        {
            var question = _dbContext.Questions.FirstOrDefault(x => x.Number == number);

            if (question == null) return null;

            return _dbContext.Hints.FirstOrDefault(x => x.QuestionId == question.Id);
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

        public void UpdateAnswer(Answer entity)
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
