using Microsoft.AspNetCore.Components.Authorization;
using MyTestApp.Shared.Models;
using System.Net.Http.Json;
using System.Security.Claims;

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
        // DÜZELTME BURADA:
        // Sunucu "ClaimTypes.Email" kullanıyorsa, istemcide de onu aramalısınız.
        // Garanti olsun diye hem standart tipi hem de kısa adını kontrol ediyoruz.

        var email = userInfo.Claims.GetValueOrDefault(ClaimTypes.Email)
                    ?? userInfo.Claims.GetValueOrDefault("email")
                    ?? userInfo.Claims.GetValueOrDefault("Mail");

        var role = userInfo.Claims.GetValueOrDefault(ClaimTypes.Role)
                   ?? userInfo.Claims.GetValueOrDefault("role")
                   ?? userInfo.Claims.GetValueOrDefault("Position");

        if (!string.IsNullOrEmpty(email))
        {
          var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, email),
                        new Claim(ClaimTypes.Role, role ?? "")
                    };

          var claimsIdentity = new ClaimsIdentity(claims, "Custom");
          var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

          return new AuthenticationState(claimsPrincipal);
        }
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Auth Error: {ex.Message}");
    }
    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
  }

  public void NotifyUserLogin() => NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
  public void NotifyUserLogout() => NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
}