using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace B2DriverLicense.Core.Entities
{
    public class Question
    {       
        public int Id { get; set; }
        public int Number { get; set; }
        public string Content { get; set; }
        public int CorrectAnswer { get; set; }
        public int ChapterId { get; set; }
        public string ImageTitle { get; set; }
        public byte[] ImageData { get; set; }

        public Chapter Chapter { get; set; }
        public Hint Hint { get; set; }
        public ICollection<Answer> Answers { get; set; }
    }
}
