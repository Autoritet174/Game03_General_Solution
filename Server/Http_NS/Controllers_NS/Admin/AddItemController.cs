using Microsoft.AspNetCore.Mvc;
using Server_DB;
using System.Text.Json.Nodes;
using SR = General.ServerErrors.Error;

namespace Server.Http_NS.Controllers_NS.Admin;

/// <summary>
/// 
/// </summary>
public class AddItemController : ControllerBaseApi
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> Main()
    {
        JsonObject? obj = await JsonObjectExt.GetJsonObjectFromRequest(Request);
        if (obj == null)
        {
            return BadRequestWithServerError(SR.RestApiBodyEmpty);
        }

        _ = JsonObjectExt.GetString(obj, "email");
        _ = JsonObjectExt.GetString(obj, "password");
        return Ok();
    }

}
