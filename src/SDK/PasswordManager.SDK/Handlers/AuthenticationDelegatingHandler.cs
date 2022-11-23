using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace PasswordManager.SDK.Handlers;

public class AuthenticationDelegatingHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthenticationDelegatingHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        //var accessToken = await _httpContextAccessor.HttpContext!.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
        //var refreshToken = await _httpContextAccessor.HttpContext!.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

        // Set HTTP Version to 3 and request lower if not possible
        request.Version = new(3, 0);
        request.VersionPolicy = HttpVersionPolicy.RequestVersionOrLower;

        var accessToken = await _httpContextAccessor.HttpContext!.GetUserAccessTokenAsync(cancellationToken: cancellationToken);

        if (!string.IsNullOrWhiteSpace(accessToken))
            request.SetBearerToken(accessToken);

        return await base.SendAsync(request, cancellationToken);
    }
}
