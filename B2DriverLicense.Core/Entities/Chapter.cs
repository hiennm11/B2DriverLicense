using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace B2DriverLicense.Core.Entities
{
    public class Chapter
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }

        public Question Question { get; set; }
    }
}
