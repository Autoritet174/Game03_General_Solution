using Microsoft.AspNetCore.Mvc;
using Server_DB;
using System.Text.Json.Nodes;
using SR = General.ServerErrors.Error;

namespace Server.Http_NS.Controllers_NS.Admin;

public class AddItemController : ControllerBaseApi
{
    public async Task<IActionResult> Main()
    {
        JsonObject? obj = await JsonObjectExt.GetJsonObjectFromRequest(Request);
        if (obj == null)
        {
            return BadRequestWithServerError(SR.RestApiBodyEmpty);
        }

        _ = JsonObjectHelper.GetString(obj, "email");
        _ = JsonObjectHelper.GetString(obj, "password");
        return Ok();
    }

}
