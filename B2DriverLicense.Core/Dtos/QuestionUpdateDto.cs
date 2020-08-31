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
        public int CorrectAnswer { get; set; }
        [Required]
        public int ChapterId { get; set; }
    }
}
