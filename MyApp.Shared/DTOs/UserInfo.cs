namespace MyTestApp.Shared.Models;

public class UserInfo
{
  public bool IsAuthenticated { get; set; }
  public string Mail { get; set; } = string.Empty;
  public string Position { get; set; } = string.Empty;
  public Dictionary<string, string> Claims { get; set; } = new Dictionary<string, string>();
}
