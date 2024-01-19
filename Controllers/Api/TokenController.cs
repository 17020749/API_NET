using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using API_CORE.Response;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using API_CORE.Models;
using API_CORE.Service;
namespace API_CORE.Controllers.Api 
{
[Route("api/[controller]")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly BikeStoresContext _bikeStoresContext;
    private readonly ITokenService _tokenService;

    public TokenController(BikeStoresContext bikeStoresContext, ITokenService tokenService)
    {
        this._bikeStoresContext = bikeStoresContext ?? throw new ArgumentNullException(nameof(bikeStoresContext));
        this._tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
    }

    [HttpPost]
    [Route("refresh")]
    public IActionResult Refresh(TokenApi tokenApiModel)
    {
        if (tokenApiModel is null)
            return BadRequest("Invalid client request");

        string accessToken = tokenApiModel.AccessToken;
        string refreshToken = tokenApiModel.RefreshToken;

        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
        var username = principal.Identity.Name; //this is mapped to the Name claim by default

        var user = _bikeStoresContext.UserTokens.SingleOrDefault(u => u.Name == username);

        if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            return BadRequest("Invalid client request");

        var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        _bikeStoresContext.SaveChanges();

        return Ok(new AuthenticatedResponse()
        {
            Token = newAccessToken,
            RefreshToken = newRefreshToken
        });
    }

    [HttpPost, Authorize]
    [Route("revoke")]
    public IActionResult Revoke()
    {
        var username = User.Identity.Name;

        var user = _bikeStoresContext.UserTokens.SingleOrDefault(u => u.Name == username);
        if (user == null) return BadRequest();

        user.RefreshToken = null;

        _bikeStoresContext.SaveChanges();

        return NoContent();
    }
}
}