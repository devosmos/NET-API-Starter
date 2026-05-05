using System.Threading.RateLimiting;
using Asp.Versioning;
using Devosmos.ApiStarter.Api.Middleware;
using Devosmos.ApiStarter.Api.OpenApi;
using Devosmos.ApiStarter.Application;
using Devosmos.ApiStarter.Infrastructure;
using Devosmos.ApiStarter.Infrastructure.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddConfiguredKeyVault();

builder.Services.AddProblemDetails();
builder.Services.AddApplicationInsightsTelemetry();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services
    .AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Title = "Validation failed.",
                Detail = "One or more request values are invalid.",
                Status = StatusCodes.Status400BadRequest,
                Type = "https://httpstatuses.com/400",
                Instance = context.HttpContext.Request.Path
            };

            return new ObjectResult(problemDetails)
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
        };
    });

builder.Services
    .AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
    })
    .AddMvc()
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<BearerSecurityDocumentTransformer>();
});

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Authentication:Authority"];
        options.Audience = builder.Configuration["Authentication:Audience"];
        options.MapInboundClaims = false;
        options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
    });

builder.Services
    .AddAuthorizationBuilder()
    .SetFallbackPolicy(new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build());

builder.Services.AddCors(options =>
{
    options.AddPolicy("Default", policy =>
    {
        var allowedOrigins = builder.Configuration
            .GetSection("Cors:AllowedOrigins")
            .Get<string[]>() ?? [];

        if (allowedOrigins.Length > 0)
        {
            policy.WithOrigins(allowedOrigins)
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
    });
});

builder.Services.AddRateLimiter(options =>
{
    var permitLimit = builder.Configuration.GetValue("RateLimiting:PermitLimit", 100);
    var windowSeconds = builder.Configuration.GetValue("RateLimiting:WindowSeconds", 60);

    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetFixedWindowLimiter(
            context.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = permitLimit,
                QueueLimit = 0,
                Window = TimeSpan.FromSeconds(windowSeconds)
            }));
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = builder.Configuration
        .GetValue<long?>("RequestLimits:MaxRequestBodySizeBytes");
});

var app = builder.Build();

app.UseExceptionHandler();
app.UseSecurityHeaders();
app.UseHttpsRedirection();
app.UseCors("Default");
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

var exposeOpenApi = app.Configuration.GetValue(
    "OpenApi:Enabled",
    app.Environment.IsDevelopment() || app.Environment.IsStaging());

if (exposeOpenApi && !app.Environment.IsProduction())
{
    app.MapOpenApi().AllowAnonymous();
}

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false
}).AllowAnonymous();

app.MapHealthChecks("/health/ready").AllowAnonymous();

app.MapControllers();

app.Run();

public partial class Program;
