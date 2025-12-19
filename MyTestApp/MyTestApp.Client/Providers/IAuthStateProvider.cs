namespace MyTestApp.Client.Providers;

public interface IAuthStateProvider
{
  void NotifyUserLogin();
  void NotifyUserLogout();
}