using MyTestApp.Components;
using MudBlazor.Services;
using Microsoft.AspNetCore.Components;
using MyTestApp.Providers;
using Microsoft.AspNetCore.Components.Authorization;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();
builder.Services.AddMudServices();
builder.Services.AddHttpClient();
builder.Services.AddAuthorizationCore();
builder.Services.AddSingleton<CustomAuthenticationStateProvider>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(MyTestApp.Client._Imports).Assembly);

app.Run();
