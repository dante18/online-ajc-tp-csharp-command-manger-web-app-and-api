using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace EatDomicile.Web.Services.Services.Security;

public class JwtCookieHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public JwtCookieHandler(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var context = this.httpContextAccessor.HttpContext;
        if (context?.Request.Cookies.TryGetValue("AccessToken", out var token) == true)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return base.SendAsync(request, cancellationToken);
    }
}

