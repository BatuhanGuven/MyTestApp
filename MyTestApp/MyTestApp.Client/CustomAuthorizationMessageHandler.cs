using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using MyTestApp.Client.Providers;
using MyTestApp.Providers;
using System.Net.Http.Headers;
using System.Security.Claims;
namespace MyTestApp.Client;

public class CustomAuthorizationMessageHandler : DelegatingHandler
{
  private readonly ICustomAuthenticationStateProvider _customAuthenticationStateProvider;
  public CustomAuthorizationMessageHandler(ICustomAuthenticationStateProvider customAuthenticationStateProvider)
  {
    _customAuthenticationStateProvider = customAuthenticationStateProvider;
  }

  protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken)
  {
    var token = _customAuthenticationStateProvider.GetToken();

    if (!string.IsNullOrEmpty(token))
    {
      if (httpRequestMessage.Headers.Authorization == null)
      {
        httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
      }
    }
    else
    {
      Console.WriteLine("No token available; sending request without Authorization header.");
    }

    return await base.SendAsync(httpRequestMessage, cancellationToken);
  }
}