using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Server.Jwt_NS;
using System.Security.Claims;

namespace Server.Http_NS.Controllers_NS.Inventory_NS
{
    public class InventoryController(Server_DB_UserData.MongoHeroesRepository mongoRepository) : ControllerBaseApi
    {
        private readonly Server_DB_UserData.MongoHeroesRepository _mongoRepository = mongoRepository;

        [EnableRateLimiting("login")]
        [HttpPost("Heroes")]
        public async Task<IActionResult> Main()
        {
            Guid? userId = User.GetGuid();
            if (userId == null)
            {
                return Unauthorized();
            }

            List<object> result = await _mongoRepository.GetAllHeroesByUserIdAsync(userId.Value);
            return Ok(new { result });
        }
    }
}
