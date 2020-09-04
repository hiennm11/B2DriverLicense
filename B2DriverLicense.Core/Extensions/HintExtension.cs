using B2DriverLicense.Core.Dtos;
using B2DriverLicense.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace B2DriverLicense.Core.Extensions
{
    public static class HintExtension
    {
        public static HintReadDto MapToReadDto(this Hint hint)
        {
            return new HintReadDto
            {
                Id = hint.Id,
                Content = hint.Content,
                QuestionId = hint.QuestionId
            };
        }

        public static Hint CreateAnswerFromDto(this HintCreateOrUpdateDto hint)
        {
            return new Hint
            {
                Content = hint.Content,
            };
        }

        public static void UpdateAnswerFromDto(this Hint entity, HintCreateOrUpdateDto dto)
        {
            entity.Content = !string.IsNullOrWhiteSpace(dto.Content) ? dto.Content : entity.Content;
        }
    }
}
