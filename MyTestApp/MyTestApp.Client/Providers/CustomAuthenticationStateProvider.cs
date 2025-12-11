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

  public override async Task<AuthenticationState> GetAuthenticationStateAsync()
  {
    var token = await _localStorageService.GetItemAsync<string>("authToken");

    if (!string.IsNullOrWhiteSpace(token))
    {
      try
      {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var claimsIdentity = new ClaimsIdentity(jwtToken.Claims, "jwt");
        _claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
      }
      catch
      {
        await _localStorageService.RemoveItemAsync("authToken");
        _claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
      }
    }
    else
    {
      _claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
    }

    return new AuthenticationState(_claimsPrincipal);
  }

}
