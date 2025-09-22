using Application.Classroom.Commands;
using Application.Classroom.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(500)]
    [Authorize(Roles = "Teacher")]
    [Route("server/classroom")]
    [ApiController]
    public class ClassroomController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClassroomController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [Authorize(Roles = "Teacher,Student")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClassroomById(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new GetClassroomDetailsQuery(id));
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

        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<IActionResult> CreateClassroom([FromBody] CreateClassroomCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                if (!result.IsSuccess)
                {
                    return BadRequest(result.Error);
                }
                return Created();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost("add-student-to-classroom")]
        public async Task<IActionResult> AddStudentToClassroom([FromBody] AddStudentToClassroomCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                if (!result.IsSuccess)
                {
                    return BadRequest(result.Error);
                }
                return Created();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpDelete("remove-student-from-classroom")]
        public async Task<IActionResult> RemoveStudentFromClassroom([FromBody] RemoveStudentFromClassroomCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
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
