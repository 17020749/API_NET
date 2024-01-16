using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API_CORE.Models; // Đảm bảo đường dẫn đúng tới namespace của model User
using API_CORE.Services; // Đảm bảo đường dẫn đúng tới namespace của các dịch vụ

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] User loginRequest)
    {
        // Kiểm tra thông tin đăng nhập
        if ((loginRequest.UserName == "test" && loginRequest.Password == "test") || (loginRequest.UserName == "admin" && loginRequest.Password  =="admin"))
        {
            // Tạo token
            var token = GenerateToken(loginRequest.UserName);
            // Trả về token
            return Ok(new { Token = token });
        }
        // Đăng nhập không thành công
        return Unauthorized(new { Message = "Invalid username or password" });
    }

    private string GenerateToken(string userName)
    {
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),   
                new Claim(ClaimTypes.Role, userName),
                new Claim("Permission", "Get"),
                new Claim("Permission", "Create"),
                new Claim("Permission", "Update"),
                new Claim("Permission", "Delete"),
            }),
            Expires = DateTime.UtcNow.AddMinutes(5),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
