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
        IEnumerable<QuestionReadDto> GetQuestionsPaging(int page, int pageSize, bool include = false);
        QuestionReadDto GetQuestionByNumber(int number, bool include = false);
        Question GetQuestionEntityByNumber(int number, bool include = false);
        QuestionReadDto GetQuestionById(int id, bool include = false);
        void CreateQuestion(Question question);
        void UpdateQuestion(Question question);
        void DeleteQuestionByDto(Question question);
        void DeleteQuestionByNumber(int number);

    }

    public class QuestionRepository : RepositoryBase<Question>, IQuestionRepository
    {
        public QuestionRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public void CreateQuestion(Question question)
        {
            if(question == null)
            {
                throw new ArgumentNullException(nameof(question));
            }

            _dbContext.Add(question);
        }

        public void DeleteQuestionByDto(Question question)
        {
            if (question == null)
            {
                throw new ArgumentNullException(nameof(question));
            }

            _dbContext.Remove(question);
        }

        public void DeleteQuestionByNumber(int number)
        {
            var question = _dbContext.Questions.FirstOrDefault(x => x.Number == number);

            if (question == null)
            {
                throw new ArgumentNullException(nameof(question));
            }

            _dbContext.Remove(question);
        }

        public QuestionReadDto GetQuestionById(int id, bool include = false)
        {
            var question = _dbContext.Questions;
            if (include)
            {
                question.Include(x => x.Answers).Include(s => s.Hint);
            }

            var result = question.FirstOrDefault(x => x.Id == id);
            return new QuestionReadDto
            {
                Id = result.Id,
                Content = result.Content,
                Number = result.Number,
                CorrectAnswer = result.CorrectAnswer,
                ChapterId = result.ChapterId
            };
        }

        public QuestionReadDto GetQuestionByNumber(int number, bool include = false)
        {
            var question = _dbContext.Questions;
            if(include)
            {
                question.Include(x => x.Answers).Include(s => s.Hint);
            }

            var result = question.FirstOrDefault(x => x.Number == number);
            if(result != null)
            {
                return new QuestionReadDto
                {
                    Id = result.Id,
                    Content = result.Content,
                    Number = result.Number,
                    CorrectAnswer = result.CorrectAnswer,
                    ChapterId = result.ChapterId
                };
            }
            return null;
        }

        public Question GetQuestionEntityByNumber(int number, bool include = false)
        {
            var question = _dbContext.Questions;
            if (include)
            {
                question.Include(x => x.Answers).Include(s => s.Hint);
            }

            var result = question.FirstOrDefault(x => x.Number == number);

            if(result != null)
            {
                return result;
            }
            
            return null;
        }

        public IEnumerable<QuestionReadDto> GetQuestionsPaging(int page, int pageSize, bool include = false)
        {
            var question = _dbContext.Questions;
            if (include)
            {
                question.Include(x => x.Answers).Include(s => s.Hint);
            }

            var result = question.Skip((page - 1) * pageSize).Take(pageSize);
            return result.Select(x=> new QuestionReadDto { Id = x.Id, Content = x.Content, Number = x.Number }).ToList();
        }

        public void UpdateQuestion(Question question)
        {
            if (question == null)
            {
                throw new ArgumentNullException(nameof(question));
            }

            _dbContext.Update(question);
            _dbContext.Entry(question).State = EntityState.Modified;
        }
    }
}
