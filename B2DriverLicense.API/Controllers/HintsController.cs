using B2DriverLicense.Core.Dtos;
using B2DriverLicense.Core.Entities;
using B2DriverLicense.Core.Extensions;
using B2DriverLicense.Service;
using B2DriverLicense.Service.Repositories;
using Microsoft.AspNetCore.Http;
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

        [HttpGet(Name = "GetHint")]
        public async Task<IActionResult> GetHint(int number)
        {
            try
            {
                var hint = await _repository.GetHintAsync(number);
                if(hint == null)
                {
                    return NotFound();
                }
                return Ok(hint.MapToReadDto());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateHint(int number, HintCreateOrUpdateDto hint)
        {
            try
            {
                if (hint == null)
                {
                    return BadRequest("Hint is required");
                }

                var question = await _repository.GetQuestionByNumberAsync(number, true);

                if (question == null)
                {
                    return NotFound($"Could not find any question number: {number}");
                }

                var existing = await _repository.GetHintAsync(number);

                if (existing != null)
                {
                    return BadRequest("Hint in use");
                }

                var hintEntity = hint.CreateAnswerFromDto();

                question.Hint = hintEntity;

                _repository.Update(question);

                if (!await _unitOfWork.SaveChangeAsync())
                {
                    return BadRequest();
                }

                return CreatedAtRoute(nameof(GetHint), 
                                      new { number = question.Number }, 
                                      hintEntity.MapToReadDto());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateHint(int number, HintCreateOrUpdateDto hint)
        {
            try
            {
                if (hint == null)
                {
                    return BadRequest("Hint is required");
                }

                var question = await _repository.GetQuestionByNumberAsync(number, true);

                if (question == null)
                {
                    return NotFound($"Could not find any question number: {number}");
                }

                var existing = await _repository.GetHintAsync(number);

                if (existing == null)
                {
                    return BadRequest($"Could not find any hint of question number: {number}");
                }

                existing.UpdateAnswerFromDto(hint);

                _repository.Update(question);

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

        [HttpDelete]
        public async Task<IActionResult> DeleteHint(int number)
        {
            try
            {
                var question = _repository.GetQuestionByNumberAsync(number, true);

                if (question == null)
                {
                    return NotFound($"Could not find any question number: {number}");
                }

                var existing = await _repository.GetHintAsync(number);

                if (existing == null)
                {
                    return BadRequest($"Could not find any hint of question number: {number}");
                }

                _repository.DeleteHint(existing);

                if (!await _unitOfWork.SaveChangeAsync())
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
