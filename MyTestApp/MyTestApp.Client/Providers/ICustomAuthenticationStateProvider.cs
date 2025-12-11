using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MyTestApp.Client.Providers;

public interface ICustomAuthenticationStateProvider
{
  public void MarkUserAsAuthenticated(string token);
  public void MarkUserAsLoggedout();
  public ClaimsPrincipal GetClaimsPrincipal();
  public string GetToken();
}
