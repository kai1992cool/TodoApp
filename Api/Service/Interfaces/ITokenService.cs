using System.Security.Claims;

namespace Service.Interfaces
{
    public interface ITokenService
    {
        public string GenerateRefreshToken(int userId);

        public string GenerateAccessToken(int userId);

        public bool ValidateRefreshToken(dynamic refreshToken);
    }
}
