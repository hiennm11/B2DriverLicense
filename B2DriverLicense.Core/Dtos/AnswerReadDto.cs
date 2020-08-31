using System;
using System.Collections.Generic;
using System.Text;

namespace B2DriverLicense.Core.Dtos
{
    public class AnswerReadDto
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public int Key { get; set; }
        public string Content { get; set; }
    }
}
