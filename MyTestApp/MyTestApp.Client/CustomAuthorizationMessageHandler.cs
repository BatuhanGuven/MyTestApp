using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using MyTestApp.Client.Providers;
using MyTestApp.Providers;
using System.Net.Http.Headers;
using System.Security.Claims;
namespace MyTestApp.Client;

public class CustomAuthorizationMessageHandler:DelegatingHandler
{
  private readonly ICustomAuthenticationStateProvider _customAuthenticationStateProvider;
  private string _token;  
  public CustomAuthorizationMessageHandler(ICustomAuthenticationStateProvider customAuthenticationStateProvider)
  {
    _customAuthenticationStateProvider = customAuthenticationStateProvider;
    _token = _customAuthenticationStateProvider.GetToken();
  }
  public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken)
  {
    if(_token is not null)
    {
      httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
      return await base.SendAsync(httpRequestMessage, cancellationToken);
    }
    else
    {
      Console.WriteLine("Token olmadığı için istek API ye gönderilemedi");
      return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
    }
  }
}