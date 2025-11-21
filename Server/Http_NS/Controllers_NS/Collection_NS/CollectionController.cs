using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Server.Jwt_NS;

namespace Server.Http_NS.Controllers_NS.Collection_NS;

public class CollectionController(Server_DB_UserData.MongoRepository mongoRepository) : ControllerBaseApi
{
    private readonly Server_DB_UserData.MongoRepository _mongoRepository = mongoRepository;

    [EnableRateLimiting("login")]
    [HttpPost("Heroes")]
    public async Task<IActionResult> Heroes()
    {
        Guid? userId = User.GetGuid();
        if (userId == null)
        {
            return Unauthorized();
        }

        Task<List<object>> taskHeroes = _mongoRepository.GetHeroesByUserIdAsync(userId.Value);
        //Task<List<object>> taskHeroes = _mongoRepository.GetHeroesByUserIdAsync(userId.Value);

        List<object> heroes = await taskHeroes;
        //List<object> result = await taskHeroes;
        return Ok(new { heroes });
    }
}
