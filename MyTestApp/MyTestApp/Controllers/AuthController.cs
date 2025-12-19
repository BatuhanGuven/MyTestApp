using Microsoft.AspNetCore.Mvc;
using MyTestApp.Shared.Models;
using MyTestApp.Services;
using System.Security.Claims;

namespace MyTestApp.Controller
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly ITokenService _tokenService;

    public AuthController(ITokenService tokenService)
    {
      _tokenService = tokenService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel model)
    {
      if (model.Mail != "test@mail.com" || model.Password != "123456")
      {
        return Unauthorized("Kullanıcı adı veya parola hatalı.");
      }

      string token = _tokenService.GetToken(model.Mail, "Admin", model.RememberMe);

      var expires = model.RememberMe  
          ? DateTimeOffset.UtcNow.AddDays(7)
          : DateTimeOffset.UtcNow.AddHours(1);

      CookieOptions cookieOptions = new()
      {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Strict,
        Expires = expires
      };

      HttpContext.Response.Cookies.Append("bearerToken", token, cookieOptions);

      return Ok();
    }

    [HttpGet("user-info")]
    public IActionResult GetUserInfo()
    {
      if (User.Identity is { IsAuthenticated: true })
      {
        var userInfo = new UserInfo
        {
          IsAuthenticated = true,
          Mail = User.FindFirstValue(ClaimTypes.Email),
          Position = User.FindFirstValue(ClaimTypes.Role),
          Claims = User.Claims.ToDictionary(c => c.Type, c => c.Value)
        };
        return Ok(userInfo);
      }
      return Ok(new UserInfo { IsAuthenticated = false });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
      HttpContext.Response.Cookies.Delete("bearerToken", new CookieOptions
      {
        Secure = true,
        HttpOnly = true,
        SameSite = SameSiteMode.Strict,
        Path = "/"
      });

      return Ok(new { Message = "Çıkış başarılı." });
    }
  }
}