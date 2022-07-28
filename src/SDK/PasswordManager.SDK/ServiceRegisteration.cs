using Microsoft.Extensions.DependencyInjection;
using PasswordManager.Logging;
using Refit;

namespace PasswordManager.SDK;

public static class ServiceRegisteration
{
    public static void AddRefitHttpClients(this IServiceCollection services)
    {
        services.AddRefitClient<IPasswordManagerApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:7001"))
            .AddHttpMessageHandler<LoggingDelegatingHandler>();
    }
}
