using Consul;

namespace PasswordManager.SDK;

internal class ConsulRegistryService
{
    private readonly IConsulClient _consulClient;
    private string serviceName = "PasswordManager.API";

    public ConsulRegistryService(IConsulClient consulClient)
    {
        _consulClient = consulClient ?? throw new ArgumentNullException(nameof(consulClient));
    }

    public Uri GetServiceUri()
    {
        var serviceQueryResult = _consulClient.Health.Service(serviceName).Result;

        if (serviceQueryResult is not null && serviceQueryResult.Response is not null && serviceQueryResult.Response.Length > 0)
        {
            var services = serviceQueryResult.Response;
            return new Uri($"https://{services[0].Service.Address}:{services[0].Service.Port}");
        }

        return null!;
    }
}
