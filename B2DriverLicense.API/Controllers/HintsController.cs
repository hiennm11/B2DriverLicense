using B2DriverLicense.Service;
using B2DriverLicense.Service.Repositories;
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
        private readonly IQuestionRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public HintsController(IQuestionRepository repository, IUnitOfWork unitOfWork)
        {
            this._repository = repository;
            this._unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetHint(int number)
        {
            var hint = await _repository.GetHintAsync(number);
            return Ok(hint);
        }
    }
}
