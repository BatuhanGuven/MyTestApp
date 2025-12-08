using Microsoft.AspNetCore.Mvc;
using MyTestApp.Models;
using MyTestApp.Services;
using System.IdentityModel.Tokens.Jwt;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
  public ITokenService _tokenService;
  public IConfiguration _configuration;
  public JwtSecurityTokenHandler JwtSecurityTokenHandler { get; set; } 
  public AuthController(ITokenService tokenService, IConfiguration configuration)
  {
    _tokenService = tokenService;
    _configuration = configuration;
  }

  [HttpPost("login")]
  [IgnoreAntiforgeryToken]
  public async Task<IActionResult> Login([FromBody] LoginModel model)
  {
    if (model.Mail != "test@mail.com" || model.Password != "123456")
    {
      return Unauthorized("Kullanıcı adı veya parola hatalı.");
    }

    string token = _tokenService.GetToken($"{model.Mail}",$"Admin",_configuration);

    return Ok(new { Token = token, Message = "Giriş başarılı." });
  }
}