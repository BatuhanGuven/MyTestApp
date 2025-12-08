using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyTestApp.Services;

public class TokenService : ITokenService
{
  public string GetToken(string mail, string role, IConfiguration configuration)
  {
    SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:JwtSecurtityKey"]));
    SigningCredentials signingCredentials = new SigningCredentials(securityKey, "HS256");
    var claims = new[]
    {
      new Claim("mail",$"{mail}"),
      new Claim("role",$"{role}")
    };
    JwtSecurityToken token = new JwtSecurityToken(
      issuer: configuration["Jwt:Issuer"],
      signingCredentials: signingCredentials,
      audience: configuration["Jwt:Audience"],
      claims: claims
     );
    return new JwtSecurityTokenHandler().WriteToken(token);
  }
}