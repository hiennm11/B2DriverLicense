using System;
using System.Collections.Generic;
using System.Text;

namespace B2DriverLicense.Core.Dtos
{
    public class UserLoginDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }

    }
}
