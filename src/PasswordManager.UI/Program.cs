using Elastic.Apm.NetCoreAll;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using PasswordManager.Logging;
using PasswordManager.SDK;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ConfigureEndpointDefaults(o => o.Protocols = HttpProtocols.Http1AndHttp2AndHttp3);
    options.ConfigureHttpsDefaults(o => o.AllowAnyClientCertificate());
});

builder.Host.UseSerilog(Serilogger.Configure);

// Add services to the container.
IdentityModelEventSource.ShowPII = true;
JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
})
    .AddCookie("Cookies", options =>
    {
        options.Events.OnSigningOut = async e =>
        {
            await e.HttpContext.RevokeUserRefreshTokenAsync();
        };
    })
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = builder.Configuration.GetValue<string>("ExternalLinks:IdentityServer");

        options.ClientId = "blazorWebClient";
        options.ClientSecret = "secret";
        options.ResponseType = "code";

        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("passManagerApi");
        options.Scope.Add("offline_access");

        options.GetClaimsFromUserInfoEndpoint = true;
        options.SaveTokens = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = JwtClaimTypes.GivenName,
            RoleClaimType = JwtClaimTypes.Role,
            ValidIssuer = "https://localhost"
        };
    });

var serviceName = Assembly.GetCallingAssembly().GetName().Name;

builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder =>
{
    tracerProviderBuilder
        .AddJaegerExporter(o =>
        {
            o.AgentHost = "localhost";
            o.AgentPort = 6831;
            o.ExportProcessorType = OpenTelemetry.ExportProcessorType.Simple;
        })
        .AddSource(serviceName)
        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName))
        .AddHttpClientInstrumentation()
        .AddAspNetCoreInstrumentation();
});

builder.Services.AddControllersWithViews();

builder.Services.AddRefitHttpClients();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseAllElasticApm(builder.Configuration);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
