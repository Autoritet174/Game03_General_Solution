using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using L = General.LocalizationKeys;

namespace Server.Http_NS.Controllers_NS;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[RequestSizeLimit(1_048_576)] // 1 МБ
public abstract class ControllerBaseApi : ControllerBase
{
   
    protected IActionResult BadRequestWithServerError(string keyError)
    {
        return BadRequest(new { keyError });
    }

    protected IActionResult BadRequestAuthInvalidCredentials()
    {
        return BadRequestWithServerError(L.Error.Server.InvalidCredentials);
    }

    protected IActionResult BadRequestInvalidResponse()
    {
        return BadRequestWithServerError(L.Error.Server.InvalidResponse);
    }

}
