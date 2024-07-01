using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Service.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Service.Services
{
    public class TokenServices : ITokenService
    {
        private readonly IConfiguration _config;
        public TokenServices(IConfiguration config) {
            _config = config;
        }
        public string GenerateRefreshToken(int userId)
        {
            var refreshToken = new {
                UserId = userId,
                IssuedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(_config.GetValue<int>("RefreshTokenExpiryDays"))
            };

            string jsonString = JsonConvert.SerializeObject(refreshToken);
            var tokenString = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(jsonString));
            return tokenString;
        }

        public string GenerateAccessToken(int userId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub,userId.ToString())
            };

            var refreshToken = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims: claims,
                //expires: DateTime.Now.AddHours(1),
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(refreshToken);
        }

        public bool ValidateRefreshToken(dynamic token)
        {
            if(token != null && token?.ExpiresAt > DateTime.UtcNow)
            {
                return true;
            }
            return false;
        }
    }
}
