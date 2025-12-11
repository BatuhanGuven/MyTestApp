using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using MyTestApp.Client.Providers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MyTestApp.Providers;

public class CustomAuthenticationStateProvider: AuthenticationStateProvider, ICustomAuthenticationStateProvider
{

  private ClaimsPrincipal _claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
  private ILocalStorageService _localStorageService;
  public CustomAuthenticationStateProvider(ILocalStorageService localStorageService)
  {
    _localStorageService = localStorageService;
  }
  public async Task MarkUserAsAuthenticated(string token)
  {
    var handler = new JwtSecurityTokenHandler();
    var jwtToken = handler.ReadJwtToken(token);
    var claimsIdentity = new ClaimsIdentity(jwtToken.Claims, "jwt");
    await _localStorageService.SetItemAsync("authToken",token);
    _claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
    NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_claimsPrincipal)));
  }

  public async Task MarkUserAsLoggedout()
  {
    _claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
    await _localStorageService.SetItemAsync("authToken", "");
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
