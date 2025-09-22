using Application.Common.Interfaces;
using Application.User.Command;
using Application.User.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    [Route("server/auth")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("sign-in")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SignIn([FromBody] SignInQuery query)
        {
            try
            {
                var result = await _userService.SignIn(query.Email, query.Password);
                if (!result.IsSuccess)
                {
                    return BadRequest(result.Error);
                }
                return Ok(result.Value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("sign-up-teacher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SignUpTeacher([FromBody] SignUpTeacherCommand command)
        {
            try
            {
                var result = await _userService.SignUpTeacher(command.Email,command.Name, command.Surname, command.Password, command.PasswordRepeated);
                if (!result.IsSuccess)
                {
                    return BadRequest(result.Error);
                }
                return Ok(result.Value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("sign-up-student")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SignUpStudent([FromBody] SignUpStudentCommand command)
        {
            try
            {
                var result = await _userService.SignUpStudent(command.Email, command.Name, command.Surname, command.Password, command.PasswordRepeated);
                if (!result.IsSuccess)
                {
                    return BadRequest(result.Error);
                }
                return Ok(result.Value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpDelete]
        public async Task<IActionResult> DeleteUser([FromBody] DeleteUserCommand command)
        {
            try
            {
                var result = await _userService.DeleteUser(command.Password);
                if (!result.IsSuccess)
                {
                    return BadRequest(result.Error);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
