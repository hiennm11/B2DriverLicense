using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using B2DriverLicense.Core.Dtos;
using B2DriverLicense.Core.Entities;
using B2DriverLicense.Service;
using B2DriverLicense.Service.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace B2DriverLicense.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswersController : ControllerBase
    {
        private readonly IAnswerRepository _answerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AnswersController(IAnswerRepository answerRepository, IUnitOfWork unitOfWork)
        {
            this._answerRepository = answerRepository;
            this._unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetAllPaging(int page = 1, int pageSize = 10)
        {
            var response = _answerRepository.GetAnswersPaging(page, pageSize);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpGet("{id}", Name = "GetByAnswerId")]
        public IActionResult GetByAnswerId(int id)
        {
            var response = _answerRepository.GetAnswerByAnswerId(id);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }
        
        [HttpGet("question-number/{number}")]
        public IActionResult GetByQuestionNumber(int number)
        {
            var response = _answerRepository.GetAnswersByQuestionNumber(number);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpGet("question-id/{id}")]
        public IActionResult GetByQuestionId(int id)
        {
            var response = _answerRepository.GetAnswersByQuestionId(id);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpPost("question-id/{id}")]
        public IActionResult CreateAnswer(int id, AnswerCreateDto answer)
        {
            try
            {
                var entity = new Answer { Content = answer.Content, Key = answer.Key, QuestionId = answer.QuestionId };
                
                _answerRepository.CreateAnswerByQuestionId(id, entity);

                if (!_unitOfWork.SaveChange())
                {
                    return BadRequest();
                }

                return CreatedAtRoute(nameof(GetByAnswerId),
                                      new { id = entity.Id },
                                      new AnswerReadDto
                                      {
                                          Id = entity.Id,
                                          Content = entity.Content,
                                          Key = entity.Key,
                                          QuestionId = entity.QuestionId
                                      });
            }
            catch (Exception ex)
            {                
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
