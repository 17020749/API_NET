using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using API_CORE.Response;
using System.Security.Claims;
using System.Text;
using API_CORE.Models;
using API_CORE.Service;
namespace API_CORE.Controllers.Api{
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly BikeStoresContext _bikeStoresContext;
    private readonly ITokenService _tokenService;

    public AuthController(BikeStoresContext bikeStoresContext, ITokenService tokenService)
    {
        _bikeStoresContext = bikeStoresContext ?? throw new ArgumentNullException(nameof(bikeStoresContext));
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
    }

    [HttpPost, Route("login")]
    public IActionResult Login([FromBody] UserToken loginModel)
    {
        if (loginModel is null)
        {
            return BadRequest("Invalid client request");
        }

        var user = _bikeStoresContext.UserTokens.FirstOrDefault(u => 
            (u.Name == loginModel.Name) && (u.Password == loginModel.Password));
        if (user is null)
            return Unauthorized();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, loginModel.Name),
            new Claim(ClaimTypes.Role, "Manager")
        };
        var accessToken = _tokenService.GenerateAccessToken(claims);
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

        _bikeStoresContext.SaveChanges();

        return Ok(new AuthenticatedResponse
        {
            Token = accessToken,
            RefreshToken = refreshToken
        });
    }
}
}

