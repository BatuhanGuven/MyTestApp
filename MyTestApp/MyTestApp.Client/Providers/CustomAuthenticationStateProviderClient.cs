using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Security.Claims;
using MyTestApp.Shared.Models;
using System.Reflection;

namespace MyTestApp.Client.Providers;

public class CustomAuthenticationStateProviderClient : AuthenticationStateProvider, IAuthStateProvider
{
  private readonly HttpClient _httpClient;

  public CustomAuthenticationStateProviderClient(IHttpClientFactory httpClientFactory)
      => _httpClient = httpClientFactory.CreateClient("PrivateAPI") ?? throw new ArgumentNullException(nameof(httpClientFactory));

  public override async Task<AuthenticationState> GetAuthenticationStateAsync()
  {
    try
    {
      var userInfo = await _httpClient.GetFromJsonAsync<UserInfo>("/api/Auth/user-info");

      if (userInfo is not null && userInfo.IsAuthenticated)
      {
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, userInfo.Claims.GetValueOrDefault("Mail")),
                new Claim(ClaimTypes.Role, userInfo.Claims.GetValueOrDefault("Position"))
            };

        var claimsIdentity = new ClaimsIdentity(claims, "Custom");
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        return new AuthenticationState(claimsPrincipal);
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex);
    }
    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
  }
  public void NotifyUserLogin()
  {
    NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
  }
  public void NotifyUserLogout()
  {
    NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
  }
}
