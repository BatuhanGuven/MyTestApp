using Microsoft.AspNetCore.Components.Authorization;
using MyTestApp.Client.Providers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MyTestApp.Providers;

public class CustomAuthenticationStateProvider: AuthenticationStateProvider, ICustomAuthenticationStateProvider
{
  private ClaimsPrincipal _claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());

  public void MarkUserAsAuthenticated(string token)
  {
    var handler = new JwtSecurityTokenHandler();
    var jwtToken = handler.ReadJwtToken(token);
    var claimsIdentity = new ClaimsIdentity(jwtToken.Claims, "jwt");

    _claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

    NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_claimsPrincipal)));
  }

  public void MarkUserAsLoggedout()
  {
    _claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
    NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_claimsPrincipal)));
  }
  public ClaimsPrincipal GetClaimsPrincipal()
  {
    return _claimsPrincipal;
  }

  public override Task<AuthenticationState> GetAuthenticationStateAsync()
  {
    return Task.FromResult(new AuthenticationState(_claimsPrincipal));
  }
}
