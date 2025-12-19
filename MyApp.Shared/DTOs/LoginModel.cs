namespace MyTestApp.Shared.Models;

public class LoginModel
{
  public required string Mail { get; set; }
  public required string Password { get; set; }
  public bool RememberMe { get; set; }
}
