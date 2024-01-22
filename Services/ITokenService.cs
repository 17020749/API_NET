using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
namespace API_CORE.Service
{
    public interface ITokenService
{
    string GenerateAccessToken(IEnumerable<Claim> claims);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
}