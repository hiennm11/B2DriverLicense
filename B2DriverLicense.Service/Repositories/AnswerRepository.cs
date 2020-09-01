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
        IEnumerable<AnswerReadDto> GetAnswersPaging(int page, int pageSize);
        IEnumerable<AnswerReadDto> GetAnswersByQuestionNumber(int number);
        IEnumerable<AnswerReadDto> GetAnswersByQuestionId(int id);
        AnswerReadDto GetAnswerByAnswerId(int id);
        void CreateAnswerByQuestionId(int questionId, Answer answer);

    }

    public class AnswerRepository : RepositoryBase<Answer>, IAnswerRepository
    {
        public AnswerRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public void CreateAnswerByQuestionId(int questionId, Answer answer)
        {
            var question = _dbContext.Questions.FirstOrDefault(x => x.Id == questionId);

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

            question.Answers.Add(answer);

            _dbContext.Questions.Update(question);
            _dbContext.Entry(question).State = EntityState.Modified;
        }

        public AnswerReadDto GetAnswerByAnswerId(int id)
        {
            var answer = _dbContext.Answers.FirstOrDefault(s => s.Id == id);

            if (answer == null) return null;

            return new AnswerReadDto
            {
                Id = answer.Id,
                Content = answer.Content,
                Key = answer.Key,
                QuestionId = answer.QuestionId
            };
        }

        public IEnumerable<AnswerReadDto> GetAnswersByQuestionId(int id)
        {
            return _dbContext.Answers.Where(s=>s.QuestionId == id).Select(x => new AnswerReadDto
            {
                Id = x.Id,
                Content = x.Content,
                Key = x.Key,
                QuestionId = x.QuestionId
            });
        }

        public IEnumerable<AnswerReadDto> GetAnswersByQuestionNumber(int number)
        {
            var question = _dbContext.Questions.FirstOrDefault(x => x.Number == number);

            if (question == null) return null;

            return _dbContext.Answers.Where(s => s.QuestionId == question.Id).Select(x => new AnswerReadDto
            {
                Id = x.Id,
                Content = x.Content,
                Key = x.Key,
                QuestionId = x.QuestionId
            });
        }

        public IEnumerable<AnswerReadDto> GetAnswersPaging(int page, int pageSize)
        {
            return _dbContext.Answers.Skip((page - 1) * pageSize).Take(pageSize).Select(x => new AnswerReadDto
            {
                Id = x.Id,
                Content = x.Content,
                Key = x.Key,
                QuestionId = x.QuestionId
            });
        }
    }
}
