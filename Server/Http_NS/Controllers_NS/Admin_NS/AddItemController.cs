using Microsoft.AspNetCore.Mvc;
using Server.Utilities;
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
        JsonObject? obj = await JsonObjectExtension.GetJsonObjectFromRequest(Request);
        if (obj == null)
        {
            return BadRequestInvalidResponse();
        }

        _ = JsonObjectExtension.GetString(obj, "email");
        _ = JsonObjectExtension.GetString(obj, "password");
        return Ok();
    }

}
