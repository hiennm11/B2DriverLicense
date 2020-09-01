using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace B2DriverLicense.Core.Dtos
{
    public class AnswerCreateDto
    {
        [Required]
        public int QuestionId { get; set; }
        [Required]
        public int Key { get; set; }
        [Required]
        public string Content { get; set; }
    }
}
