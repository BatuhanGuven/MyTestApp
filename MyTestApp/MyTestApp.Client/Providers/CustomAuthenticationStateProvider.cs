using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Security.Claims;
using MyTestApp.Shared.Models;
namespace MyTestApp.Client.Providers;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
  private readonly HttpClient _httpClient;

  public CustomAuthenticationStateProvider(IHttpClientFactory httpClientFactory)
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
                new(ClaimTypes.Email, userInfo.Mail),
                new(ClaimTypes.Role, userInfo.Position)
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
