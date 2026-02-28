namespace GarageVelo.Services.Api;

public class AuthenticatedHttpClientHandler : DelegatingHandler
{
    private readonly ISessionService _sessionService;

    public AuthenticatedHttpClientHandler(ISessionService sessionService)
        : base(new HttpClientHandler())
    {
        _sessionService = sessionService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _sessionService.GetTokenAsync();
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
