using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly;
using MyTestApp.Providers;
using MyTestApp.Client;
using Microsoft.Extensions.DependencyInjection;
var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped<CustomAuthorizationMessageHandler>();

builder.Services.AddHttpClient("ServerAPI", client =>
{
  client.BaseAddress = new Uri(builder.Configuration["BaseAdress"] ?? builder.HostEnvironment.BaseAddress);
}).AddHttpMessageHandler<CustomAuthorizationMessageHandler>();
  
await builder.Build().RunAsync();
