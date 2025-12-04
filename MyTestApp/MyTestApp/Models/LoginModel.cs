using System.ComponentModel.DataAnnotations;

namespace MyTestApp.Models;

public class LoginModel
{
  public required string Mail { get; set; }  
  public required string Password { get; set; }
}
