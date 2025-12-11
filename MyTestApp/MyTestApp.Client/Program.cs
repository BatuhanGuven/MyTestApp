using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly;
using MyTestApp.Providers;
using MyTestApp.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;
using MyTestApp.Client.Providers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped<CustomAuthorizationMessageHandler>();
builder.Services.AddMudServices();
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();


builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<ICustomAuthenticationStateProvider>(sp => sp.GetRequiredService<CustomAuthenticationStateProvider>());
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<CustomAuthenticationStateProvider>());
    
builder.Services.AddHttpClient("ServerAPI", client =>
{
  client.BaseAddress = new Uri(builder.Configuration["BaseAdress"] ?? builder.HostEnvironment.BaseAddress);
}).AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

builder.Services.AddHttpClient("PublicAPI", client =>
{
  client.BaseAddress = new Uri(builder.Configuration["BaseAdress"] ?? builder.HostEnvironment.BaseAddress);
});


await builder.Build().RunAsync();