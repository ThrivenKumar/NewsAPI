using Microsoft.AspNetCore.Mvc;
using NewsAPI.Services;
using NewsAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity.Data;

namespace NewsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;


        public AuthController(ITokenService tokenService) 
        {
            _tokenService = tokenService;
        }
        

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginCredentials request)
        {
            if (request.username != "testuser" || request.password != "testpassword")
            {
                return Unauthorized();
            }
            else
            {
                string token = _tokenService.GenerateToken(request.username);
                return Ok(new { Token = token });
            }
        }
    }
}
