using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace B2DriverLicense.Core.Entities
{
    public class Hint
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Content { get; set; }

        public Question Question { get; set; }
    }
}
