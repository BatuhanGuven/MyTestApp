using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using MyTestApp.Client.Providers;
using MyTestApp.Providers;
using System.Security.Claims;
namespace MyTestApp.Client;

public class CustomAuthorizationMessageHandler
{
  private readonly ICustomAuthenticationStateProvider _customAuthenticationStateProvider;
  private ClaimsPrincipal _claimsPrincipal;
  public CustomAuthorizationMessageHandler(ICustomAuthenticationStateProvider customAuthenticationStateProvider, HttpClient httpClient)
  {
    _customAuthenticationStateProvider = customAuthenticationStateProvider;
    _customAuthenticationStateProvider.GetClaimsPrincipal();
  }
  public async Task SendAsync()
  {
    if(_claimsPrincipal is not null)
    {

    }
  }
}