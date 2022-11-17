using IdentityServer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using PasswordManager.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ConfigureEndpointDefaults(o => o.Protocols = HttpProtocols.Http1AndHttp2AndHttp3);
    options.ConfigureHttpsDefaults(o => o.AllowAnyClientCertificate());
});

builder.Host.UseSerilog(Serilogger.Configure);

var app = builder
    .ConfigureServices()
    .ConfigurePipeline();

if (args.Contains("/seed"))
{
    SeedData.EnsureSeedData(app);
    return;
}

app.Run();
