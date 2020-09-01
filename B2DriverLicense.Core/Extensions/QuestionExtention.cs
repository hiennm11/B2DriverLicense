using B2DriverLicense.Core.Dtos;
using B2DriverLicense.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace B2DriverLicense.Core.Extensions
{
    public static class QuestionExtention
    {
        public static QuestionReadDto MapToReadModel(this Question question)
        {
            var answers = new List<AnswerReadDto>();
            if(question.Answers != null)
            {
                answers = question.Answers.Select(x => new AnswerReadDto
                {
                    Id = x.Id,
                    Content = x.Content,
                    Key = x.Key,
                    QuestionId = x.QuestionId
                }).ToList();
            }

            return new QuestionReadDto
            {
                Id = question.Id,
                Answers = answers,
                ChapterId = question.ChapterId,
                Content = question.Content,
                CorrectAnswer = question.CorrectAnswer,
                Number = question.Number
            };
        }

        public static List<QuestionReadDto> MapToListReadModel(this IEnumerable<Question> questions)
        {
            return questions.Select(x => new QuestionReadDto
            {
                Id = x.Id,
                Answers = x.Answers != null ? x.Answers.Select(a => new AnswerReadDto
                {
                    Id = a.Id,
                    Content = a.Content,
                    Key = a.Key,
                    QuestionId = a.QuestionId
                }).ToList() : new List<AnswerReadDto>(),
                ChapterId = x.ChapterId,
                Content = x.Content,
                CorrectAnswer = x.CorrectAnswer,
                Number = x.Number
            }).ToList();
        }

        public static Question MapCreateDtoToEntity(this QuestionCreateDto dto)
        {
            return new Question
            {
                Number = dto.Number,
                Content = dto.Content,
                CorrectAnswer = dto.CorrectAnswer,
                ChapterId = dto.ChapterId
            };
        }
    }
}
