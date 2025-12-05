using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MyTestApp.Providers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddSingleton<CustomAuthenticationStateProvider>();
await builder.Build().RunAsync();
