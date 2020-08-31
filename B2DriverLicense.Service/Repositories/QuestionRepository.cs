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
        IEnumerable<QuestionReadDto> GetQuestionsPaging(int page, int pageSize, bool include = false, int chapterId = 0);
        QuestionReadDto GetQuestionByNumber(int number, bool include = false);
        Question GetQuestionEntityByNumber(int number, bool include = false);
        QuestionReadDto GetQuestionById(int id, bool include = false);
        Question GetQuestionEntityById(int id, bool include = false);
        void CreateQuestion(Question question);
        void UpdateQuestion(Question question);
        void DeleteQuestionByDto(Question question);
        void DeleteQuestionById(int id);
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

        public void DeleteQuestionById(int id)
        {
            var question = _dbContext.Questions.FirstOrDefault(x => x.Id == id);

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
            var result = _dbContext.Questions.FirstOrDefault(x => x.Id == id);
            var answers = new List<AnswerReadDto>();

            if (result == null)
            {
                return null;
            }

            if (include)
            {
                _dbContext.Entry(result).Reference(x => x.Chapter).Load();
                _dbContext.Entry(result).Collection(x => x.Answers).Load();

                answers = result.Answers.Select(a => new AnswerReadDto
                {
                    Id = a.Id,
                    Content = a.Content,
                    Key = a.Key,
                    QuestionId = a.QuestionId
                }).ToList();
            }

            return new QuestionReadDto
            {
                Id = result.Id,
                Content = result.Content,
                Number = result.Number,
                CorrectAnswer = result.CorrectAnswer,
                ChapterId = result.ChapterId,
                Answers = answers
            };
        }

        public QuestionReadDto GetQuestionByNumber(int number, bool include = false)
        {
            var result = _dbContext.Questions.FirstOrDefault(x => x.Number == number);
            var answers = new List<AnswerReadDto>();
            if (result == null)
            {
                return null;
            }

            if (include)
            {
                _dbContext.Entry(result).Reference(x => x.Chapter).Load();
                _dbContext.Entry(result).Collection(x => x.Answers).Load();

                answers = result.Answers.Select(a => new AnswerReadDto
                {
                    Id = a.Id,
                    Content = a.Content,
                    Key = a.Key,
                    QuestionId = a.QuestionId
                }).ToList();
            }

            return new QuestionReadDto
            {
                Id = result.Id,
                Content = result.Content,
                Number = result.Number,
                CorrectAnswer = result.CorrectAnswer,
                ChapterId = result.ChapterId, 
                Answers = answers
            };
        }

        public Question GetQuestionEntityById(int id, bool include = false)
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

        public Question GetQuestionEntityByNumber(int number, bool include = false)
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

        public IEnumerable<QuestionReadDto> GetQuestionsPaging(int page, int pageSize, bool include = false, int chapterId = 0)
        {
            var question = _dbContext.Questions.AsQueryable();

            if(chapterId > 0)
            {
                question = question.Where(x => x.ChapterId == chapterId);
            }

            if (include)
            {
                question.Include(x => x.Answers);
            }

            var result = question.Skip((page - 1) * pageSize).Take(pageSize);
            return result.Select(x => new QuestionReadDto
            {
                Id = x.Id,
                Content = x.Content,
                Number = x.Number,
                CorrectAnswer = x.CorrectAnswer,
                ChapterId = x.ChapterId,
                Answers = include ? x.Answers.Select(a => new AnswerReadDto
                {
                    Id = a.Id,
                    Content = a.Content,
                    Key = a.Key,
                    QuestionId = a.QuestionId
                }).ToList() : new List<AnswerReadDto>()
            }).ToList();
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
