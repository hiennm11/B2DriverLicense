﻿using System;
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
        private readonly IQuestionRepository _questionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AnswersController(IQuestionRepository questionRepository, IUnitOfWork unitOfWork)
        {
            this._questionRepository = questionRepository;
            this._unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetByQuestionNumber(int number)
        {
            try
            {
                var response = await _questionRepository.GetAnswersByQuestionNumberAsync(number);

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
        public async Task<IActionResult> GetByAnswerKey(int number, int key)
        {
            try
            {
                var response = await _questionRepository.GetAnswerAsync(number, key);

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
        public async Task<IActionResult> CreateAnswer(int number, AnswerCreateOrEditDto answer)
        {
            try
            {
                var answerEntity = answer.CreateAnswerFromDto();

                var question = await _questionRepository.GetQuestionByNumberAsync(number, true);

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

                var existing = await _questionRepository.GetAnswerAsync(number, answer.Key);

                if (existing != null)
                {
                    return BadRequest("Answer in use");
                }

                question.Answers.Add(answerEntity);

                _questionRepository.Update(question);

                if (!await _unitOfWork.SaveChangeAsync())
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
        public async Task<IActionResult> UpdateAnswer(int number, int key, AnswerCreateOrEditDto answer)
        {
            try
            {
                var question = await _questionRepository.GetQuestionByNumberAsync(number, true);

                if (question == null)
                {
                    return NotFound($"Could not find any question number: {number}");
                }

                if (answer == null)
                {
                    return BadRequest("Answer is required");
                }

                var answerToUpdate = await _questionRepository.GetAnswerAsync(number, key);

                if (answerToUpdate == null)
                {
                    return BadRequest($"Could not find any answer with key of {key}");
                }

                answerToUpdate.UpdateAnswerFromDto(answer);
                _questionRepository.UpdateAnswer(answerToUpdate);

                if (!await _unitOfWork.SaveChangeAsync())
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
        public async Task<IActionResult> DeteleAnswer(int number, int key)
        {
            try
            {
                var question = _questionRepository.GetQuestionByNumberAsync(number, true);

                if (question == null)
                {
                    return NotFound($"Could not find any question number: {number}");
                }

                var answerToDelete = await _questionRepository.GetAnswerAsync(number, key);

                if (answerToDelete == null)
                {
                    return BadRequest($"Could not find any answer with key of {key}");
                }

                _questionRepository.DeleteAnswer(answerToDelete);

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
