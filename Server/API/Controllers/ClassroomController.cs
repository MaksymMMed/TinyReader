using Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("server/[controller]")]
    [ApiController]
    public class ClassroomController : ControllerBase
    {
        private readonly IClassroomService _service;

        public ClassroomController(IClassroomService service)
        {
            _service = service;
        }
    }
}
