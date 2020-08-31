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
            var response = _questionRepository.GetQuestionsPaging(page, pageSize, include, chapterId);

            if(response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpGet("id/{questionId}", Name = "GetQuestionById")]
        public IActionResult GetQuestionById(int questionId, bool include = false)
        {
            var response = _questionRepository.GetQuestionById(questionId, include);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpGet("number/{number}", Name = "GetQuestionByNumber")]
        public IActionResult GetQuestionByNumber(int number, bool include = false)
        {
            var response = _questionRepository.GetQuestionByNumber(number, include);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpPost]
        public IActionResult CreateQuestion(QuestionCreateDto question)
        {
            try
            {
                var entity = new Question
                {
                    Number = question.Number,
                    Content = question.Content,
                    CorrectAnswer = question.CorrectAnswer,
                    ChapterId = question.ChapterId
                };

                _questionRepository.CreateQuestion(entity);
                
                if (!_unitOfWork.SaveChange())
                {
                    return BadRequest();
                }

                return CreatedAtRoute(nameof(GetQuestionByNumber),
                                      new { number = question.Number },
                                      new QuestionReadDto
                                      {
                                          Id = entity.Id,
                                          ChapterId = entity.ChapterId,
                                          Content = entity.Content,
                                          CorrectAnswer = entity.CorrectAnswer,
                                          Number = entity.Number
                                      });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error!");
                
            }
        }

        [HttpPut("id/{questionId}")]
        public IActionResult UpdateQuestionById(int questionId, QuestionUpdateDto question)
        {
            var questionEntity = _questionRepository.GetQuestionEntityById(questionId);

            if (questionEntity == null)
            {
                return NotFound();
            }

            try
            {
                var entity = questionEntity;
                entity.Content = question.Content;
                entity.CorrectAnswer = question.CorrectAnswer;
                entity.ChapterId = question.ChapterId;

                _questionRepository.UpdateQuestion(entity);

                if (!_unitOfWork.SaveChange())
                {
                    return BadRequest();
                }

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error!");
            }
        }


        [HttpPut("number/{number}")]
        public IActionResult UpdateQuestionByNumber(int number, QuestionUpdateDto question)
        {
            var questionEntity = _questionRepository.GetQuestionEntityByNumber(number);

            if(questionEntity == null)
            {
                return NotFound();
            }

            try
            {
                var entity = questionEntity;
                entity.Content = question.Content;
                entity.CorrectAnswer = question.CorrectAnswer;
                entity.ChapterId = question.ChapterId;

                _questionRepository.UpdateQuestion(entity);
                
                if (!_unitOfWork.SaveChange())
                {
                    return BadRequest();
                }
                
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error!");
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
                _questionRepository.DeleteQuestionById(questionId);

                if (!_unitOfWork.SaveChange())
                {
                    return BadRequest();
                }

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error!");

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
                _questionRepository.DeleteQuestionByNumber(number);

                if (!_unitOfWork.SaveChange())
                {
                    return BadRequest();
                }

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error!");

            }
        }
    }
}
