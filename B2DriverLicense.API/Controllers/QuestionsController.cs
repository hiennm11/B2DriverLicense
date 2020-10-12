using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using B2DriverLicense.Core.Dtos;
using B2DriverLicense.Core.Entities;
using B2DriverLicense.Core.Extensions;
using B2DriverLicense.Service;
using B2DriverLicense.Service.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace B2DriverLicense.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "admin")]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public QuestionsController(IQuestionRepository repository, IUnitOfWork unitOfWork)
        {
            this._repository = repository;
            this._unitOfWork = unitOfWork;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int page = 1, int pageSize = 10, bool include = false, int chapterId = 0)
        {
            try
            {
                var response = await _repository.GetQuestionsPagingAsync(page, pageSize, include, chapterId);

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
        [AllowAnonymous]
        public async Task<IActionResult> GetQuestionByNumber(int number, bool include = false)
        {
            var response = await _repository.GetQuestionByNumberAsync(number, include);

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
                var existing = _repository.GetQuestionByNumberAsync(question.Number);

                if (existing != null) return BadRequest("Question is use");

                var entity = question.MapCreateDtoToEntity();

                await _repository.AddAsync(entity);
                
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
            var questionEntity = await _repository.GetQuestionByNumberAsync(number);

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

                _repository.Update(entity);
                
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
            var questionEntity = await _repository.GetQuestionByNumberAsync(number);

            if (questionEntity == null)
            {
                return NotFound();
            }

            try
            {
                _repository.Delete(questionEntity);

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
