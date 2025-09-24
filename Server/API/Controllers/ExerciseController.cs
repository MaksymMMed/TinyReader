using Infrastructure.RabbitMq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(500)]
   // [Authorize(Roles = "Teacher")]
    [Route("server/exercises")]
    [ApiController]
    public class ExerciseController : ControllerBase
    {
        private readonly IRabbitAudioService _audioService;

        public ExerciseController(IRabbitAudioService audioService)
        {
            _audioService = audioService;
        }

        [HttpPost("exercise-audio")]
        public async Task<IActionResult> PostExerciseAudio(IFormFile audioFile)
        {
            try
            {
                var response = await _audioService.SendAudioAsync(audioFile);
                if (response.IsSuccess)
                {
                    return Ok(response.Value);
                }
                else
                {
                    return BadRequest(response.Error);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
