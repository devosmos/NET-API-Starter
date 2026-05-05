namespace Devosmos.ApiStarter.Api.Middleware;

public sealed class SecurityHeadersMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.OnStarting(() =>
        {
            var headers = context.Response.Headers;

            headers.TryAdd("X-Content-Type-Options", "nosniff");
            headers.TryAdd("X-Frame-Options", "DENY");
            headers.TryAdd("Referrer-Policy", "no-referrer");
            headers.TryAdd("X-Permitted-Cross-Domain-Policies", "none");
            headers.TryAdd("Permissions-Policy", "accelerometer=(), camera=(), geolocation=(), gyroscope=(), magnetometer=(), microphone=(), payment=(), usb=()");

            if (context.Request.IsHttps)
            {
                headers.TryAdd("Strict-Transport-Security", "max-age=31536000; includeSubDomains");
            }

            return Task.CompletedTask;
        });

        await next(context);
    }
}
