using Microsoft.AspNetCore.Mvc;

namespace Server.Http_NS.Controllers_NS.Http_Tests;

/// <summary>
/// 
/// </summary>
public class TestController : ControllerBaseApi
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public IActionResult Main()
    {
        string blabla = "ЫЫЫЫЫ";
        return Ok(new { blabla });
    }
}
