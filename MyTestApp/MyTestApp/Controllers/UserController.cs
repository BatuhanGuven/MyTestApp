using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTestApp.Shared.Models;

namespace MyTestApp.Controller;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
  [Authorize]
  [HttpGet("users")]
  public List<UserDto> GetUsersAsync()
  {
    return new List<UserDto>
    {
      new() {Name = "Batuhan", Position = "IT"},
      new() {Name = "Tunahan", Position = "Arch"}
    };
  }
}
