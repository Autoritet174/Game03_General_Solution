using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;

namespace Server.Http_NS.Controllers_NS.Admin_NS;

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
            return BadRequestInvalidResponse();
        }

        _ = JsonObjectExt.GetString(obj, "email");
        _ = JsonObjectExt.GetString(obj, "password");
        return Ok();
    }

}
