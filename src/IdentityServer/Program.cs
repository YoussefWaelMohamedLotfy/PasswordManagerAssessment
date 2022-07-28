using IdentityServer;
using PasswordManager.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(Serilogger.Configure);

var app = builder
    .ConfigureServices()
    .ConfigurePipeline();

app.Run();
