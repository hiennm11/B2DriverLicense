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
        public async Task<IActionResult> Get(int page = 1, int pageSize = 10, bool include = false, int chapterId = 0)
        {
            try
            {
                var response = await _questionRepository.GetQuestionsPagingAsync(page, pageSize, include, chapterId);

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
        
        [HttpGet("{number:int}", Name = "GetQuestionByNumber")]
        public async Task<IActionResult> GetQuestionByNumber(int number, bool include = false)
        {
            var response = await _questionRepository.GetQuestionByNumberAsync(number, include);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response.MapToReadModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuestion(QuestionCreateDto question)
        {
            try
            {
                var existing = _questionRepository.GetQuestionByNumberAsync(question.Number);

                if (existing != null) return BadRequest("Question is use");

                var entity = question.MapCreateDtoToEntity();

                await _questionRepository.AddAsync(entity);
                
                if (!(await _unitOfWork.SaveChangeAsync()))
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

        [HttpPut("{number:int}")]
        public async Task<IActionResult> UpdateQuestionByNumber(int number, QuestionUpdateDto question)
        {
            var questionEntity = await _questionRepository.GetQuestionByNumberAsync(number);

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
                
                if (!await _unitOfWork.SaveChangeAsync())
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

        [HttpDelete("{number:int}")]
        public async Task<IActionResult> DeleteQuestionByNumber(int number)
        {
            var questionEntity = await _questionRepository.GetQuestionByNumberAsync(number);

            if (questionEntity == null)
            {
                return NotFound();
            }

            try
            {
                _questionRepository.Delete(questionEntity);

                if (!await _unitOfWork.SaveChangeAsync())
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
