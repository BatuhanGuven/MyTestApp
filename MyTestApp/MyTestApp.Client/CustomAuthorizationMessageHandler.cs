using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

using System.Net.Http.Headers;
using System.Security.Claims;
namespace MyTestApp.Client;

public class CustomAuthorizationMessageHandler : DelegatingHandler
{
  private readonly ILocalStorageService _localStorageService;
  public CustomAuthorizationMessageHandler(ILocalStorageService localStorageService)
  {
    _localStorageService = localStorageService;
  }

  protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken)
  {
    var token = await _localStorageService.GetItemAsync<string>("authToken");

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