using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using MyTestApp.Client.Providers;
using MyTestApp.Client.Service;
using MyTestApp.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddMudServices();
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

// register provider as both the framework type and the custom interface
builder.Services.AddScoped<CustomAuthenticationStateProviderClient>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<CustomAuthenticationStateProviderClient>());
builder.Services.AddScoped<IAuthStateProvider>(sp => sp.GetRequiredService<CustomAuthenticationStateProviderClient>());

// service registrations
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<AntiForgeryTokenHandler>();
builder.Services.AddHttpClient("PrivateAPI", client =>
{
  client.BaseAddress = new Uri(builder.Configuration["BaseAddress"] ?? builder.HostEnvironment.BaseAddress);
}).AddHttpMessageHandler<AntiForgeryTokenHandler>();
await builder.Build().RunAsync();