using B2DriverLicense.Core.Dtos;
using B2DriverLicense.Core.EF;
using B2DriverLicense.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace B2DriverLicense.Service.Repositories
{
    public interface IAnswerRepository : IRepositoryBase<Answer>
    {
        IEnumerable<Answer> GetAnswersPaging(int page, int pageSize);
        IEnumerable<Answer> GetAnswersByQuestionNumber(int number);
        IEnumerable<Answer> GetAnswersByQuestionId(int id);
        Answer GetAnswer(int number, int key);
        Answer GetAnswerByAnswerId(int id);
        void AddAnswerToQuestion(int number, Answer answer);

    }

    public class AnswerRepository : RepositoryBase<Answer>, IAnswerRepository
    {
        public AnswerRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public void AddAnswerToQuestion(int number, Answer answer)
        {
            var question = _dbContext.Questions.FirstOrDefault(x => x.Number == number);

            if (question == null)
            {
                throw new ArgumentNullException(nameof(question));
            }

            if (answer == null)
            {
                throw new ArgumentNullException(nameof(answer));
            }

            if (question.Answers == null)
            {
                question.Answers = new List<Answer>();
            }

            var existing = _dbContext.Answers.FirstOrDefault(x => x.Key == answer.Key);

            if(existing != null)
            {

            }

            question.Answers.Add(answer);

            _dbContext.Questions.Update(question);
            _dbContext.Entry(question).State = EntityState.Modified;
        }

        public Answer GetAnswer(int number, int key)
        {
            var question = _dbContext.Questions.FirstOrDefault(x => x.Number == number);

            if(question == null)
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

        public Answer GetAnswerByAnswerId(int id)
        {
            var answer = _dbContext.Answers.FirstOrDefault(s => s.Id == id);

            if (answer == null) return null;

            return answer;
        }

        public IEnumerable<Answer> GetAnswersByQuestionId(int id)
        {
            return _dbContext.Answers.Where(s=>s.QuestionId == id);
        }

        public IEnumerable<Answer> GetAnswersByQuestionNumber(int number)
        {
            var question = _dbContext.Questions.FirstOrDefault(x => x.Number == number);

            if (question == null) return null;

            return _dbContext.Answers.Where(s => s.QuestionId == question.Id);
        }

        public IEnumerable<Answer> GetAnswersPaging(int page, int pageSize)
        {
            return _dbContext.Answers.Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
}
