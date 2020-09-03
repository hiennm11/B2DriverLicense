using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace B2DriverLicense.Core.Dtos
{
    public class QuestionUpdateDto
    {
        [Required]
        public string Content { get; set; }
        [Required]
        [RegularExpression("([1-9][0-9]*)")]
        public int CorrectAnswer { get; set; }
        [Required]
        [RegularExpression("([1-9][0-9]*)")]
        public int ChapterId { get; set; }
    }
}
