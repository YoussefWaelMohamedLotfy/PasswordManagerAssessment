using Microsoft.Extensions.Hosting;
using Serilog;

namespace PasswordManager.Logging;

public static class Serilogger
{
    public static Action<HostBuilderContext, LoggerConfiguration> Configure =>
       (context, configuration) =>
       {
           configuration
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                .Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName)
                .ReadFrom.Configuration(context.Configuration);
       };
}