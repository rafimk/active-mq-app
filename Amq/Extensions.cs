using ActiveMQ.Artemis.Client;
using ActiveMQ.Artemis.Client.Extensions.DependencyInjection;
using ActiveMQ.Artemis.Client.Extensions.Hosting;

namespace active_mq_app.Amq;

public static class Extensions
{
    public static IServiceCollection AddActiveMQ(IServiceCollection services)
    {
        var endpoints = new[] { ActiveMQ.Artemis.Client.Endpoint.Create(host: "localhost", port: 5672, "admin", "admin") };
        services.AddActiveMq("bookstore-cluster", endpoints);
        services.AddActiveMqHostedService();
        return services;
    }
}