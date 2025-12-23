using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace MyTestApp.Providers;

public class CustomAuthenticationStateProviderServer : AuthenticationStateProvider
{
  private readonly IHttpContextAccessor _httpContextAccessor;
  public CustomAuthenticationStateProviderServer(IHttpContextAccessor httpContextAccessor)
  {
    _httpContextAccessor = httpContextAccessor;
  }

  public override Task<AuthenticationState> GetAuthenticationStateAsync()
  {
    ClaimsPrincipal principal = _httpContextAccessor.HttpContext?.User ?? new ClaimsPrincipal(new ClaimsIdentity());
    return Task.FromResult(new AuthenticationState(principal));
  }
}
