using Microsoft.AspNetCore.Mvc;

namespace Server.Http_NS.Controllers_NS.Http_Tests;

public class TestController : ControllerBaseApi
{
    [HttpPost]
    //[AllowAnonymous]
    public IActionResult Main()
    {
        string blabla = "ЫЫЫЫЫ";
        return Ok(new { blabla });
    }
}
