namespace MyTestApp.Services;

interface ITokenService
{
  string GetToken(string mail, string role, IConfiguration configuration);
}