using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using B2DriverLicense.Core.Entities;
using B2DriverLicense.Service.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace B2DriverLicense.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionRepository _questionRepository;

        public QuestionsController(IQuestionRepository questionRepository)
        {
            this._questionRepository = questionRepository;
        }

        [HttpGet]
        public IActionResult Get(int page = 1, int pageSize = 10, bool include = false)
        {
            var response = _questionRepository.GetQuestionsPaging(page, pageSize, include);
            return Ok(response);
        }
    }
}
