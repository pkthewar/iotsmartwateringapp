using Microsoft.AspNetCore.Mvc;
using SmartPlantWaterer.Helpers;
using SmartPlantWaterer.Models;

namespace SmartPlantWaterer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            if (loginDto == null)
                return BadRequest();

            if (loginDto.UserName != "admin" || loginDto.Password != "admin")
                return Unauthorized();

            string token = JwtHelper.GenerateToken(loginDto.UserName);

            return Ok(token);
        }
    }
}
