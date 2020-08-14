using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace B2DriverLicense.Core.Entities
{
    public class Question
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }
        public int CorrectAnswer { get; set; }
        public int ChapterId { get; set; }

        public Chapter Chapter { get; set; }
        public ICollection<Answer> Answers { get; set; }
    }
}
