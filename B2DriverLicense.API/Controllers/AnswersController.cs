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
    [Route("api/questions/{number}/answers")]
    [ApiController]
    public class AnswersController : ControllerBase
    {
        private readonly IAnswerRepository _answerRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AnswersController(IAnswerRepository answerRepository, IQuestionRepository questionRepository, IUnitOfWork unitOfWork)
        {
            this._answerRepository = answerRepository;
            this._questionRepository = questionRepository;
            this._unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetByQuestionNumber(int number)
        {
            try
            {
                var response = _answerRepository.GetAnswersByQuestionNumber(number);

                if (response == null)
                {
                    return NotFound();
                }

                return Ok(response.MapToListReadDto());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpGet("{key:int}", Name = "GetByAnswerKey")]
        public IActionResult GetByAnswerKey(int number, int key)
        {
            try
            {
                var response = _answerRepository.GetAnswer(number, key);

                if (response == null)
                {
                    return NotFound();
                }

                return Ok(response.MapToReadDto());
            }
            catch (Exception)
            { 
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPost]
        public IActionResult CreateAnswer(int number, AnswerCreateOrEditDto answer)
        {
            try
            {
                var answerEntity = answer.CreateAnswerFromDto();

                var question = _questionRepository.GetQuestionByNumber(number, true);

                if (question == null)
                {
                    return NotFound($"Could not find any question number: {number}");
                }

                if (answer == null)
                {
                    return BadRequest("Answer is required");
                }

                if (question.Answers == null)
                {
                    question.Answers = new List<Answer>();
                }

                var existing = _answerRepository.GetAnswer(number, answer.Key);

                if (existing != null)
                {
                    return BadRequest("Answer in use");
                }

                question.Answers.Add(answerEntity);

                _questionRepository.Update(question);

                if (!_unitOfWork.SaveChange())
                {
                    return BadRequest();
                }

                return CreatedAtRoute(nameof(GetByAnswerKey),
                                      new { number = question.Number, key = answerEntity.Key },
                                      answerEntity.MapToReadDto());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }

        [HttpPut("{key:int}")]
        public IActionResult UpdateAnswer(int number, int key, AnswerCreateOrEditDto answer)
        {
            try
            {
                var question = _questionRepository.GetQuestionByNumber(number, true);

                if (question == null)
                {
                    return NotFound($"Could not find any question number: {number}");
                }

                if (answer == null)
                {
                    return BadRequest("Answer is required");
                }

                var answerToUpdate = _answerRepository.GetAnswer(number, key);

                if (answerToUpdate == null)
                {
                    return BadRequest($"Could not find any answer with key of {key}");
                }

                answerToUpdate.UpdateAnswerFromDto(answer);
                _answerRepository.Update(answerToUpdate);

                if (!_unitOfWork.SaveChange())
                {
                    return BadRequest();
                }

                return NoContent();

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }

        [HttpDelete("{key:int}")]
        public IActionResult DeteleAnswer(int number, int key)
        {
            try
            {
                var question = _questionRepository.GetQuestionByNumber(number, true);

                if (question == null)
                {
                    return NotFound($"Could not find any question number: {number}");
                }

                var answerToDelete = _answerRepository.GetAnswer(number, key);

                if (answerToDelete == null)
                {
                    return BadRequest($"Could not find any answer with key of {key}");
                }

                _answerRepository.Delete(answerToDelete);

                if (!_unitOfWork.SaveChange())
                {
                    return BadRequest();
                }

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }
    }
}
