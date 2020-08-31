using System;
using System.Collections.Generic;
using System.Text;

namespace B2DriverLicense.Core.Dtos
{
    public class QuestionReadDto
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Content { get; set; }
        public int CorrectAnswer { get; set; }
        public int ChapterId { get; set; }

        public List<AnswerReadDto> Answers { get; set; }
    }
}
