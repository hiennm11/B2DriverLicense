using System;
using System.Collections.Generic;
using System.Text;

namespace B2DriverLicense.Core.Dtos
{
    public class HintReadDto
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Content { get; set; }
    }
}
