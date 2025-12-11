using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTestApp.Client.Models;
using MyTestApp.Models;
namespace MyTestApp.Controller;

[Route("api/[controller]")]
[ApiController]
public class UserController: ControllerBase
{
  [HttpGet("users")]
  [Authorize]
  public async Task<List<User>> GetUsersAsync()
  {
    return new List<User>
    {
      new User {Name = "Batuhan", Position = "IT"},
      new User {Name = "Tunahan", Position = "Arch"}
    };
  }
}
