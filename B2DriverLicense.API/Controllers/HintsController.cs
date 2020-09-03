using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace B2DriverLicense.API.Controllers
{
    [Route("api/questions/{number:int}/hint")]
    [ApiController]
    public class HintsController : ControllerBase
    {
        public HintsController()
        {

        }
    }
}
