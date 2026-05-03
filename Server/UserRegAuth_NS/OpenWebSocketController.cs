using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Server.UserRegAuth_NS;

[ApiController]
[Route("api/[controller]")]
public class OpenWebSocketController() : ControllerBase
{
    [HttpPost("OpenWebSocket")]
    [AllowAnonymous]
    public IActionResult Open()
    {
        return Ok();
    }
}
