using System;
using System.Collections.Generic;
namespace API_CORE.Models;

public partial class UserToken
{   
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } 

    public string Password { get; set; } = null!;

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiryTime { get; set; }
}
