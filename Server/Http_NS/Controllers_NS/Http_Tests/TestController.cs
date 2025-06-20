using General.DataBaseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Jwt_NS;

namespace Server.Http_NS.Controllers_NS.Http_Tests;

public class TestController: ControllerBaseApi {
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Main() {
        string blabla = "ЫЫЫЫЫ";
        return Ok(new { blabla });
    }
}
