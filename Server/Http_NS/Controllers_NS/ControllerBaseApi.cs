using Google.Rpc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Server.Http_NS.Controllers_NS;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public abstract class ControllerBaseApi : ControllerBase
{
    protected IActionResult ApiOk(object data)
    {
        return Ok(new { Success = true, Data = data });
    }

    protected IActionResult ApiError(string message)
    {
        return BadRequest(new { Success = false, Error = message });
    }

    protected IActionResult CBA_BadRequest(General.GF.ServerErrors serverError) {
        return BadRequest(new { code = serverError });
    }
}
