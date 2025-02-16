using Microsoft.AspNetCore.Mvc;

namespace Bazingo_API.Controllers
{
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        [Route("/")]
        public IActionResult Root()
        {
            return Ok("Root endpoint is working!");
        }

        [HttpGet]
        [Route("/test")]
        public IActionResult Test()
        {
            return Ok("Test endpoint is working!");
        }

        [HttpGet]
        [Route("/hello")]
        public IActionResult Hello()
        {
            return Ok("Hello from the API!");
        }
    }
}
