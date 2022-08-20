using Consul;
using System.Reflection;

namespace PasswordManager.API.Services;

public class ConsulDiscoveryService : IHostedService
{
    private readonly IConsulClient _consulClient;
    private readonly ILogger<ConsulDiscoveryService> _logger;

    string serviceName = Assembly.GetCallingAssembly().GetName().Name!;

    public ConsulDiscoveryService(IConsulClient consulClient, ILogger<ConsulDiscoveryService> logger)
    {
        _consulClient = consulClient ?? throw new ArgumentNullException(nameof(consulClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _consulClient.Agent.ServiceDeregister($"{serviceName}-1", cancellationToken);

        var serviceUri = new Uri("https://localhost:7001");

        var serviceRegistration = new AgentServiceRegistration()
        {
            ID = $"{serviceName}-1",
            Address = serviceUri.Host,
            Name = serviceName,
            Port = serviceUri.Port,
            Tags = new[] { "API", "PassManager" }
        };
        
        await _consulClient.Agent.ServiceRegister(serviceRegistration, cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _consulClient.Agent.ServiceDeregister($"{serviceName}-1", cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError("Deregistering Error", ex);
        }
    }
}
