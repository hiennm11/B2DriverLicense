using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using B2DriverLicense.Core.Dtos;
using B2DriverLicense.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace B2DriverLicense.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            this._userService = userService;
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody]UserLoginDto user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var tokenResult = await _userService.Authenticate(user);
                if (string.IsNullOrWhiteSpace(tokenResult))
                {
                    return BadRequest("UserName or password is incorrect.");
                }
                return Ok(new { token = tokenResult });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]UserRegisterDto user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!user.ConfirmPassword.Equals(user.Password))
                {
                    return BadRequest("Your passwords are not equals.");
                }

                var result = await _userService.Register(user);
                if (!result)
                {
                    return BadRequest("Register is unsuccessful.");
                }
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }
    }
}
