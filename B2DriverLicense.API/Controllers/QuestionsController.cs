using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using B2DriverLicense.Core.Dtos;
using B2DriverLicense.Core.Entities;
using B2DriverLicense.Core.Extensions;
using B2DriverLicense.Service;
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
        private readonly IUnitOfWork _unitOfWork;

        public QuestionsController(IQuestionRepository questionRepository, IUnitOfWork unitOfWork)
        {
            this._questionRepository = questionRepository;
            this._unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Get(int page = 1, int pageSize = 10, bool include = false, int chapterId = 0)
        {
            try
            {
                var response = _questionRepository.GetQuestionsPaging(page, pageSize, include, chapterId);

                if (response == null)
                {
                    return NotFound();
                }

                return Ok(response.MapToListReadModel());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpGet("id/{questionId}", Name = "GetQuestionById")]
        public IActionResult GetQuestionById(int questionId, bool include = false)
        {
            var response = _questionRepository.GetQuestionById(questionId, include);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response.MapToReadModel());
        }

        [HttpGet("number/{number}", Name = "GetQuestionByNumber")]
        public IActionResult GetQuestionByNumber(int number, bool include = false)
        {
            var response = _questionRepository.GetQuestionByNumber(number, include);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response.MapToReadModel());
        }

        [HttpPost]
        public IActionResult CreateQuestion(QuestionCreateDto question)
        {
            try
            {
                var existing = _questionRepository.GetQuestionByNumber(question.Number);

                if (existing != null) return BadRequest("Question is use");

                var entity = question.MapCreateDtoToEntity();

                _questionRepository.Add(entity);
                
                if (!_unitOfWork.SaveChange())
                {
                    return BadRequest();
                }

                return CreatedAtRoute(nameof(GetQuestionByNumber),
                                      new { number = question.Number },
                                      entity.MapToReadModel());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
                
            }
        }

        [HttpPut("id/{questionId}")]
        public IActionResult UpdateQuestionById(int questionId, QuestionUpdateDto question)
        {
            var questionEntity = _questionRepository.GetQuestionById(questionId);

            if (questionEntity == null)
            {
                return NotFound($"Could not find any question with id of {questionId}");
            }

            try
            {
                var entity = questionEntity;
                entity.Content = question.Content;
                entity.CorrectAnswer = question.CorrectAnswer;
                entity.ChapterId = question.ChapterId;

                _questionRepository.Update(entity);

                if (!_unitOfWork.SaveChange())
                {
                    return BadRequest();
                }

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }


        [HttpPut("number/{number}")]
        public IActionResult UpdateQuestionByNumber(int number, QuestionUpdateDto question)
        {
            var questionEntity = _questionRepository.GetQuestionByNumber(number);

            if(questionEntity == null)
            {
                return NotFound($"Could not find any question number: {number}");
            }

            try
            {
                var entity = questionEntity;
                entity.Content = question.Content;
                entity.CorrectAnswer = question.CorrectAnswer;
                entity.ChapterId = question.ChapterId;

                _questionRepository.Update(entity);
                
                if (!_unitOfWork.SaveChange())
                {
                    return BadRequest();
                }
                
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpDelete("id/{questionId}")]
        public IActionResult DeleteQuestionById(int questionId)
        {
            var questionEntity = _questionRepository.GetQuestionById(questionId);

            if (questionEntity == null)
            {
                return NotFound();
            }

            try
            {
                _questionRepository.Delete(questionEntity);

                if (!_unitOfWork.SaveChange())
                {
                    return BadRequest();
                }

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");

            }
        }

        [HttpDelete("number/{number}")]
        public IActionResult DeleteQuestionByNumber(int number)
        {
            var questionEntity = _questionRepository.GetQuestionByNumber(number);

            if (questionEntity == null)
            {
                return NotFound();
            }

            try
            {
                _questionRepository.Delete(questionEntity);

                if (_unitOfWork.SaveChange())
                {
                    return Ok();
                }

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return BadRequest();
        }
    }
}
