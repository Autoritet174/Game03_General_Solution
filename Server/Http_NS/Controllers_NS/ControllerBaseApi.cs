using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Server.Http_NS.Controllers_NS;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public abstract class ControllerBaseApi : ControllerBase {
    protected IActionResult ApiOk(object data) => Ok(new { Success = true, Data = data });
    protected IActionResult ApiError(string message) => BadRequest(new { Success = false, Error = message });

}
