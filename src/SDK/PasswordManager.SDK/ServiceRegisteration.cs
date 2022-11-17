using Microsoft.Extensions.DependencyInjection;
using PasswordManager.Logging;
using Polly.Extensions.Http;
using Polly;
using Refit;
using Serilog;
using PasswordManager.SDK.Handlers;
using Consul;

namespace PasswordManager.SDK;

public static class ServiceRegisteration
{
    public static void AddRefitHttpClients(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddAccessTokenManagement();

        services.AddTransient<AuthenticationDelegatingHandler>();
        services.AddTransient<LoggingDelegatingHandler>();

        services.AddSingleton<IConsulClient, ConsulClient>(_ =>
            new ConsulClient(c => c.Address = new Uri("http://raspberrypi:8500")))
            .AddSingleton<ConsulRegistryService>();

        services.AddRefitClient<IPasswordManagerApi>()
            .ConfigureHttpClient((serviceProvider, client) =>
            {
                var consulService = serviceProvider.GetRequiredService<ConsulRegistryService>();
                client.BaseAddress = consulService.GetServiceUri();
            })
            // Can be used instead of AuthenticationDelegatingHandler
            //.AddUserAccessTokenHandler()
            .AddHttpMessageHandler<AuthenticationDelegatingHandler>()
            .AddHttpMessageHandler<LoggingDelegatingHandler>()
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        /* 
        * In this case will wait for
        * 2 ^ 1 = 2 seconds then
        * 2 ^ 2 = 4 seconds then
        * 2 ^ 3 = 8 seconds then
        * 2 ^ 4 = 16 seconds then
        * 2 ^ 5 = 32 seconds
        */

        => HttpPolicyExtensions.HandleTransientHttpError()
            .WaitAndRetryAsync(
                retryCount: 5,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (exception, retryCount, context) =>
                {
                    Log.Error($"Retry {retryCount} of {context.PolicyKey} at {context.OperationKey}, due to: {exception}.");
                });

    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy() 
        => HttpPolicyExtensions.HandleTransientHttpError()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromSeconds(30)
            );
}
