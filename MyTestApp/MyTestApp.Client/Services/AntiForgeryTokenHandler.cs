using Microsoft.AspNetCore.Components.Forms;

namespace MyTestApp.Client.Services;

public class AntiForgeryTokenHandler : DelegatingHandler
{
  private readonly AntiforgeryStateProvider _antiForgeryStateProvider;

  public AntiForgeryTokenHandler(AntiforgeryStateProvider antiforgeryStateProvider)
  {
    _antiForgeryStateProvider = antiforgeryStateProvider;
  }
  protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
  {
    AntiforgeryRequestToken? antiForgeryToken = _antiForgeryStateProvider.GetAntiforgeryToken();
    if(antiForgeryToken is not null)
    {
      request.Headers.Add("X-XSRF-TOKEN", antiForgeryToken.Value);
    }
    return base.SendAsync(request, cancellationToken);
  }
}
