using MyTestApp.Shared.Models;
using Shared.Models;

namespace MyTestApp.Client.Service;

public interface IAuthenticationService
{
  public Task<ServiceResponse> Login(LoginModel loginModel);
  public Task<ServiceResponse> Logout();
}
