using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace B2DriverLicense.Core.Dtos
{
    public class HintCreateOrUpdateDto
    {
        [Required]
        public string Content { get; set; }
    }
}
