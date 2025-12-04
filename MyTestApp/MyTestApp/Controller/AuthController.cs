using Microsoft.AspNetCore.Mvc;
using MyTestApp.Models;
using MyTestApp.Services;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
  ITokenService _tokenService;
  IConfiguration _configuration;

  AuthController(ITokenService tokenService, IConfiguration configuration)
  {
    _tokenService = tokenService;
    _configuration = configuration;
  }
  [HttpPost("/login")]
  public async Task<IActionResult> Login([FromBody] LoginModel model)
  {
    if (model.Mail != "test@mail.com" || model.Password != "123456")
    {
      return Unauthorized("Kullanıcı adı veya parola hatalı.");
    }

    var token = _tokenService.GetToken($"{model.Mail}",$"Admin",_configuration);

    return Ok(new { Token = token, Message = "Giriş başarılı." });
  }
}