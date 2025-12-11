using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MyTestApp.Client.Providers;

public interface ICustomAuthenticationStateProvider
{
  public Task MarkUserAsAuthenticated(string token);
  public Task MarkUserAsLoggedout();
  public ClaimsPrincipal GetClaimsPrincipal();
}
