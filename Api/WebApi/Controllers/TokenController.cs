using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Service.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenServices;

        public TokenController(IConfiguration config, ITokenService tokenServices)
        {
            _config = config;
            _tokenServices = tokenServices;
        }

        [HttpGet("{refreshToken}")]
        public IActionResult GetToken([FromRoute] string refreshToken)
        {
            string jsonString = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(refreshToken));

            var token = JsonConvert.DeserializeObject<dynamic>(jsonString)!;

            if(_tokenServices.ValidateRefreshToken(token))
            {
                var response = new{
                    jwttoken = _tokenServices.GenerateAccessToken((int)token?.UserId),
                    refreshToken = _tokenServices.GenerateRefreshToken((int)token?.UserId)
                };
                return Ok(response); 
            }
            return Unauthorized("Invalid token");
        }
    }
}