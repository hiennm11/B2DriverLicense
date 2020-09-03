using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace B2DriverLicense.Core.Dtos
{
    public class AnswerCreateOrEditDto
    {
        [Required]
        [RegularExpression("([1-9][0-9]*)")]
        public int Key { get; set; }
        [Required]
        public string Content { get; set; }
    }
}
