using System.IdentityModel.Tokens.Jwt;

namespace MyTestApp.Models;

public class LoginResponse
{
  public string Token { get; set; }  
  public string Message { get; set; } 
}
