using B2DriverLicense.Core.Dtos;
using B2DriverLicense.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace B2DriverLicense.Core.Extensions
{
    public static class AnswerExtention
    {
        public static AnswerReadDto MapToReadDto(this Answer answer)
        {
            return new AnswerReadDto
            {
                Id = answer.Id,
                Content = answer.Content,
                Key = answer.Key,
                QuestionId = answer.QuestionId
            };
        }

        public static List<AnswerReadDto> MapToListReadDto(this IEnumerable<Answer> answers)
        {
            return answers.Select(x => new AnswerReadDto
            {
                Id = x.Id,
                Content = x.Content,
                Key = x.Key,
                QuestionId = x.QuestionId
            }).ToList();
        }

        public static Answer CreateAnswerFromDto(this AnswerCreateOrEditDto answer)
        {
            return new Answer
            {
                Content = answer.Content,
                Key = answer.Key,
            };
        }

        public static void UpdateAnswerFromDto(this Answer entity, AnswerCreateOrEditDto dto)
        {
            entity.Content = !string.IsNullOrWhiteSpace(dto.Content) ? dto.Content : entity.Content;
            entity.Key = dto.Key > 0 ? dto.Key : entity.Key;
        }
    }
}
