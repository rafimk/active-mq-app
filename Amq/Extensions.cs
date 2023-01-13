using ActiveMQ.Artemis.Client.Extensions.DependencyInjection;
using ActiveMQ.Artemis.Client.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace active_mq_app.Amq;

public static class Extensions
{
    public static IServiceCollection AddActiveMQBroker(this IServiceCollection services)
    {
        var endpoints = new[] { ActiveMQ.Artemis.Client.Endpoint.Create(host: "localhost", port: 5672, "admin", "admin") };

        services.AddActiveMq("bookstore-cluster", endpoints)
                .AddAnonymousProducer<MessageProducer>();
                
        services.AddActiveMqHostedService();
        return services;
    }
}