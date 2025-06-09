using dotnet.challenge.api.Errors;
using dotnet.challenge.api.Services.Users;
using dotnet.challenge.api.Utils.Dtos;
using dotnet.challenge.api.Utils.Forms;
using dotnet.challenge.api.Utils.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotnet.challenge.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Produces("application/json")]
        [SwaggerOperation(
            Summary = "Creates a User based on provided form",
            Description = "This endpoint creates a user based on a form that is provided throught request body"
            )]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateUser([FromBody] UserForm userForm)
        {
            var validator = new UserFormValidator();
            var validationResult = await validator.ValidateAsync(userForm);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var result = (await _userService.CreateUserAsync(userForm))
                .Match(resultValue => Result<UserDto, Error>.Success(resultValue), error => error);

            if (result.Error != null)
                return Problem(result.Error.Message, null, result.Error.Code);
            else
                return CreatedAtAction(nameof(GetUser), new { id = result?.Value?.Id }, result?.Value);
        }

        [HttpGet]
        [Produces("application/json")]
        [SwaggerOperation(
            Summary = "Retrieves all Users",
            Description = "This endpoint retrieves all the users contained in cache"
            )]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUsers([FromQuery] int pageNumber = 1, int pageSize = 10)
        {
            var result = (await _userService.GetUsersAsnyc(pageNumber, pageSize))
                .Match(resultValue => Result<IEnumerable<UserDto>, Error>.Success(resultValue), error => error);

            if (result.Error != null)
                return Problem(result.Error.Message, null, result.Error?.Code);
            else
                return Ok(result?.Value);
        }

        [HttpGet]
        [Route("{id}")]
        [Produces("application/json")]
        [SwaggerOperation(
            Summary = "Retrieves an specific User",
            Description = "This endpoint retrieves an specific user based on the provided Id"
            )]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUser([FromRoute] Guid id)
        {
            var result = (await _userService.GetUserAsync(id))
               .Match(resultValue => Result<UserDto, Error>.Success(resultValue), error => error);

            if (result.Error != null)
                return Problem(result.Error.Message, null, result.Error?.Code);
            else
                return Ok(result?.Value);
        }

        [HttpDelete]
        [Route("{id}")]
        [Produces("application/json")]
        [SwaggerOperation(
            Summary = "Deletes an specific User",
            Description = "This endpoint deletes an specific user based on the provided Id"
            )]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            var result = (await _userService.DeleteUserAsync(id))
               .Match(resultValue => Result<UserDto, Error>.Success(resultValue), error => error);

            if (result.Error != null)
                return Problem(result.Error.Message, null, result.Error?.Code);
            else
                return NoContent();
        }

        [HttpPut]
        [Route("{id}")]
        [Produces("application/json")]
        [SwaggerOperation(
            Summary = "Replace an specific User",
            Description = "This endpoint replace an specific user based on the provided Id"
            )]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> PutUser([FromRoute] Guid id, [FromBody] UserForm userForm)
        {
            var validator = new UserFormValidator();
            var validationResult = await validator.ValidateAsync(userForm);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var result = (await _userService.UpdateUserAsync(id, userForm))
               .Match(resultValue => Result<UserDto, Error>.Success(resultValue), error => error);

            if (result.Error != null)
                return Problem(result.Error.Message, null, result.Error?.Code);
            else
                return NoContent();
        }
    }
}
