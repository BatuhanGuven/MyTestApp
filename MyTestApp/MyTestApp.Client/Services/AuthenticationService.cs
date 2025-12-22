using Microsoft.AspNetCore.Components.Authorization;
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
        // BURASI KRİTİK: Gelen provider'ın Client versiyonu olup olmadığını kontrol ediyoruz.
        // Eğer sunucuda çalışıyorsa bu blok güvenli bir şekilde atlanır ve hata vermez.
        if (_authStateProvider is CustomAuthenticationStateProviderClient clientProvider)
        {
          clientProvider.NotifyUserLogin();
        }

        return new ServiceResponse { Success = true };
      }
      var errorContent = await response.Content.ReadAsStringAsync();
      return new ServiceResponse { Success = false, Message = errorContent };
    }
    catch (Exception)
    {
      return new ServiceResponse { Success = false, Message = "Sunucuya bağlanılamadı." };
    }
  }

  public async Task<ServiceResponse> Logout()
  {
    try
    {
      var response = await _privateHttpClient.PostAsync("/api/Auth/logout", null);
      if (response.IsSuccessStatusCode)
      {
        if (_authStateProvider is CustomAuthenticationStateProviderClient clientProvider)
        {
          clientProvider.NotifyUserLogout();
        }
        return new ServiceResponse { Success = true };
      }
      return new ServiceResponse { Success = false, Message = await response.Content.ReadAsStringAsync() };
    }
    catch (Exception ex)
    {
      return new ServiceResponse { Success = false, Message = ex.Message };
    }
  }
}