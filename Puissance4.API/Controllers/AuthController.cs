using Be.Khunly.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Puissance4.API.DTO;

namespace Puissance4.API.Controllers
{
    [ApiController]
    public class AuthController(JwtManager _jwtManager) : ControllerBase
    {

        [HttpPost("api/login")]
        public IActionResult Login([FromBody]LoginDTO dto)
        {
            if(dto.Password != "1234")
            {
                return Unauthorized();
            }
            return Ok(new
            {
                Token = _jwtManager.CreateToken(dto.Username, dto.Username, dto.Username)
            });
        }
    }
}
