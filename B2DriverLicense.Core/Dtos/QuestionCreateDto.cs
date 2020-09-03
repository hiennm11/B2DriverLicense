using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace B2DriverLicense.Core.Dtos
{
    public class QuestionCreateDto
    {
        [Required]
        [RegularExpression("([1-9][0-9]*)")]
        public int Number { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public int CorrectAnswer { get; set; }
        [Required]
        public int ChapterId { get; set; }
    }
}
