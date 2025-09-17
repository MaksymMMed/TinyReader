using API.Models.Account;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("server/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] SignInModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.SignIn(model.Email,model.Password);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }
        [HttpPost("sign-up-teacher")]
        public async Task<IActionResult> SignUpTeacher([FromBody] SignUpModel model)
        {
            var result = await _userService.SignUpTeacher(model.Email,model.Name, model.Surname, model.Password, model.PasswordRepeated);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }
        [HttpPost("sign-up-student")]
        public async Task<IActionResult> SignUpStudent([FromBody] SignUpModel model)
        {
            var result = await _userService.SignUpStudent(model.Email, model.Name, model.Surname, model.Password, model.PasswordRepeated);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }
    }
}
