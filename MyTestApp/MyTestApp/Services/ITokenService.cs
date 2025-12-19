namespace MyTestApp.Services;

public interface ITokenService
{
  string GetToken(string mail, string position, bool rememberMe);
}