using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using MyTestApp.Client.Providers;
using MyTestApp.Shared.Models;
using Shared.Models;
using System.Net.Http.Json;

namespace MyTestApp.Client.Service;

public class AuthenticationService : IAuthenticationService
{
  private readonly HttpClient _privateHttpClient;
  private readonly AuthenticationStateProvider _authStateProvider;

  public AuthenticationService(AuthenticationStateProvider authStateProvider, IHttpClientFactory httpClientFactory)
  {
    _authStateProvider = authStateProvider;
    _privateHttpClient = httpClientFactory.CreateClient("PrivateAPI");
  }

  public async Task<ServiceResponse> Login(LoginModel loginModel)
  {
    try
    {
      var response = await _privateHttpClient.PostAsJsonAsync("/api/Auth/login", loginModel);

      if (response.IsSuccessStatusCode)
      {
        if (_authStateProvider is CustomAuthenticationStateProvider customAuthenticationStateProvider)
        {
          customAuthenticationStateProvider.NotifyUserLogin();
        }

        return new ServiceResponse { Success = true };
      }
      var errorContent = await response.Content.ReadAsStringAsync();
      return new ServiceResponse { Success = false, Message = errorContent };
    }
    catch (Exception ex)
    {
      return new ServiceResponse { Success = false, Message = "Sunucuya bağlanılamadı." };
    }
  }
  public async Task<ServiceResponse> Logout()
  {
    var response = await _privateHttpClient.PostAsync("/api/Auth/logout", null);
    try
    {
      if (response.IsSuccessStatusCode)
      {
        if(_authStateProvider is CustomAuthenticationStateProvider customAuthenticationStateProvider)
        {
          customAuthenticationStateProvider.NotifyUserLogout();
        }
        return new ServiceResponse { Success = true };
      }
      else
      {
        return new ServiceResponse { Success = false, Message = await response.Content.ReadAsStringAsync() };
      }
    }
    catch (Exception ex)
    {
      return new ServiceResponse { Success = false, Message = ex.Message };
    }
  }
}