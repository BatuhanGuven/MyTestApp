using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyTestApp.Services;

public class TokenService : ITokenService
{
  private readonly IConfiguration _configuration;
  private readonly SigningCredentials _signingCredentials;
  private readonly SymmetricSecurityKey _securityKey;
  private readonly string _issuer;
  private readonly string _audience;
  private static readonly JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();

  public TokenService(IConfiguration configuration)
  {
    _configuration = configuration;
    var key = _configuration["Jwt"] ?? throw new InvalidOperationException("Jwt: Key missing");

    _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
    _signingCredentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256);
    _issuer = _configuration["Jwt:Issuer"];
    _audience = _configuration["Jwt:Audience"];
  }

  public string GetToken(string mail, string position, bool rememberMe)
  {
    var claims = new[]
    {
        new Claim(ClaimTypes.Email, mail),
        new Claim(ClaimTypes.Role, position)
    };

    var expireTime = rememberMe ? DateTime.UtcNow.AddDays(7) : DateTime.UtcNow.AddHours(1);

    var token = new JwtSecurityToken(
      issuer: _issuer,
      signingCredentials: _signingCredentials,
      audience: _audience,
      claims: claims,
      expires: expireTime
     );

    return _tokenHandler.WriteToken(token);
  }
}