using Bazingo_Core.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace Bazingo_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        protected ActionResult<ApiResponse<T>> OkResponse<T>(T data, string message = "Operation completed successfully")
        {
            return Ok(ApiResponse<T>.CreateSuccess(data, message));
        }

        protected ActionResult<ApiResponse<T>> ErrorResponse<T>(string message, int statusCode = 400)
        {
            var response = ApiResponse<T>.CreateError(message);
            return StatusCode(statusCode, response);
        }
    }
}
